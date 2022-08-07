using Newtonsoft.Json;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Extensions;
using WordDataAccessLibrary.Helpers;

namespace WordDataAccessLibrary.CollectionBackupServices.JsonServices;
public class WordCollectionExportService : ICollectionExportService
{
    public WordCollectionExportService(IWordCollectionService collectionService)
    {
        CollectionService = collectionService;
    }

    IWordCollectionService CollectionService { get; }

    readonly string _requiredFileExtension = AppFileExtension.Get(FileExtension.Wordlist);

    public async Task<ExportActionResult> Export(List<IExportWordCollection> exportCollections, string path)
    {
        ExportActionResult result;

        path = path.Trim();
        if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
        if (Path.GetExtension(path) != _requiredFileExtension) path += _requiredFileExtension;

        result = CanCreateFolderSuccessfully(path);
        if (result.Success is false) return result;

        (result, string json) = ConvertDataToJson(exportCollections);
        if (result.Success is false) return result;
        return await WriteWordListFile(path, json);
    }

    public async Task<ExportActionResult> ExportByCollectionOwners(List<WordCollectionOwner> owners, string path)
    {
        return await ExportByCollectionOwners(owners, path, false);
    }

    public async Task<ExportActionResult> ExportByCollectionOwners(List<WordCollectionOwner> owners, string path, bool removeUserData)
    {
        List<WordCollection> collections = await CollectionService.GetWordCollectionsById(owners.Select(x => x.Id).ToArray());
        if (removeUserData)
        {
            collections = collections.ResetLearnStates();
        }
        return await Export(collections.ToIJsonCollection(), path);
    }

    public static (ExportActionResult result, string json) ConvertDataToJson(IEnumerable<IExportWordCollection> exportCollections)
    {
        ExportActionResult actionResult = new(BackupAction.ParseData)
        {
            Success = false
        };

        if (exportCollections is null || exportCollections.Any() is false)
        {
            actionResult.MoreInfo = $"{nameof(exportCollections)} should not be {(exportCollections is null ? "NULL" : "empty")}";
            return (actionResult, string.Empty);
        }
        
        JsonBackupStruct data = new(exportCollections.Select(x => (DefaultExportWordCollection)x).ToArray());
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        if (string.IsNullOrWhiteSpace(json))
        {
            actionResult.MoreInfo = $"Parsed string '{nameof(json)}' is empty or null, and should not be";
            return (actionResult, string.Empty);
        }
        actionResult.Success = true;
        return (actionResult, json);
    }

    private static async Task<ExportActionResult> WriteWordListFile(string path, string data)
    {
        ExportActionResult result = new(BackupAction.WriteFile)
        {
            Success = false,
            UsedPath = path
        };

        try
        {
            using (StreamWriter sw = new(path, false))
            {
                await sw.WriteAsync(data);
            }
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.MoreInfo = $"Exception throw whilst trying to write to file: {ex}, {ex.Message}";
#if DEBUG
            throw;
#else
            return result;
#endif
        }
    }

    private static ExportActionResult CanCreateFolderSuccessfully(string path)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            return new(BackupAction.CreateFolder)
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            ExportActionResult result = new(BackupAction.CreateFolder)
            {
                Success = false,
                UsedPath = path,
                MoreInfo = $"Exception while trying to create folder: {ex}, {ex.Message}"
            };
            Debug.WriteLine(result.MoreInfo);
#if DEBUG
            throw;
#else
            return result;
#endif
        }
    }
}


