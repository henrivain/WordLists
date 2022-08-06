using Newtonsoft.Json;
using System.Diagnostics;
using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Extensions;

namespace WordDataAccessLibrary.CollectionBackupServices.JsonServices;
public class JsonWordCollectionExportService : ICollectionExportService
{
    public JsonWordCollectionExportService(IWordCollectionService collectionService)
    {
        CollectionService = collectionService;
    }

    IWordCollectionService CollectionService { get; }

    public async Task<ExportActionResult> Export(List<IExportWordCollection> exportCollections, string path)
    {
        ExportActionResult result;

        path = path.Trim();
        if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
        if (path.EndsWith(".json") is false) path += ".json";

        result = CanCreateFolderSuccessfully(path);
        if (result.Success is false) return result;

        (string json, result) = ConvertDataToJson(exportCollections);
        if (result.Success is false) return result;
        return await WriteJsonFile(path, json);
    }

    private static async Task<ExportActionResult> WriteJsonFile(string path, string data)
    {
        try
        {
            using (StreamWriter sw = new(path, false))
            {
                await sw.WriteAsync(data);
            }
            return new(ExportAction.WriteFile)
            {
                Success = true,
                MoreInfo = "Export Successfull",
                UsedPath = path
            };
        }
        catch (Exception ex)
        {
            ExportActionResult result = new(ExportAction.WriteFile)
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
            return new(ExportAction.CreateFolder)
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            ExportActionResult result = new(ExportAction.CreateFolder)
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
        ExportActionResult actionResult = new(ExportAction.ParseData)
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

        string result = JsonConvert.SerializeObject(exportCollections.ToArray(), Formatting.Indented);
        if (string.IsNullOrWhiteSpace(result))
        {
            actionResult.MoreInfo = $"Parsed {nameof(result)} is empty or null, and should not be";
            return (string.Empty, actionResult);
        }
        actionResult.Success = true;
        return (result, actionResult);
    }

}


