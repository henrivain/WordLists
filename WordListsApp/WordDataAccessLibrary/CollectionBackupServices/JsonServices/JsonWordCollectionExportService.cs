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

    public async Task<ExportActionResult> Export(List<IExportWordCollection> exportCollections, string path)
    {
        ExportActionResult result;

        path = path.Trim();
        if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

        string requiredExtension = AppFileExtension.GetExtension(FileExtension.Wordlist);
        if (path.EndsWith(requiredExtension) is false) path += requiredExtension;

        result = CanCreateFolderSuccessfully(path);
        if (result.Success is false) return result;

        (string json, result) = ConvertDataToJson(exportCollections);
        if (result.Success is false) return result;
        return await WriteWordListFile(path, json);
    }

    private static async Task<ExportActionResult> WriteWordListFile(string path, string data)
    {
        try
        {
            using (StreamWriter sw = new(path, false))
            {
                await sw.WriteAsync(data);
            }
            return new(BackupAction.WriteFile)
            {
                Success = true,
                MoreInfo = "Export Successfull",
                UsedPath = path
            };
        }
        catch (Exception ex)
        {
            ExportActionResult result = new(BackupAction.WriteFile)
            {
                Success = false,
                MoreInfo = $"Exception throw whilst trying to write to file: {ex}, {ex.Message}",
                UsedPath = path
            };

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

    public static (string json, ExportActionResult result) ConvertDataToJson(List<IExportWordCollection> exportCollections)
    {
        ExportActionResult actionResult = new(BackupAction.ParseData)
        {
            Success = false
        };
        if (exportCollections is null)
        {
            actionResult.MoreInfo = $"{nameof(exportCollections)} should not be null";
            return (string.Empty, actionResult);
        }
        if (exportCollections.Count == 0)
        {
            actionResult.MoreInfo = $"{nameof(exportCollections)} should not be empty";
            return (string.Empty, actionResult);
        }

        JsonBackupStruct data = new(exportCollections.Select(x => (ExportWordCollection)x).ToArray());
        string result = JsonConvert.SerializeObject(data, Formatting.Indented);
        if (string.IsNullOrWhiteSpace(result))
        {
            actionResult.MoreInfo = $"Parsed {nameof(result)} is empty or null, and should not be";
            return (string.Empty, actionResult);
        }
        actionResult.Success = true;
        return (result, actionResult);
    }


   

}


