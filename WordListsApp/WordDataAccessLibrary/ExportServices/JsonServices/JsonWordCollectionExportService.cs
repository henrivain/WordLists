using Newtonsoft.Json;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Extensions;

namespace WordDataAccessLibrary.ExportServices.JsonServices;
public class JsonWordCollectionExportService : IExportService
{
    public JsonWordCollectionExportService(IWordCollectionService collectionService)
    {
        CollectionService = collectionService;
    }

    IWordCollectionService CollectionService { get; }

    public async Task<ExportActionResult> Export(List<IExportWordCollection> exportCollections, string path)
    {
        path = path.Trim();
        if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
        if (path.EndsWith(".json") is false) path += ".json";

        await Task.Delay(1);
        throw new NotImplementedException();
    }




    /// <summary>
    /// Get object as json string, Object must have name or InvalidDataException is thrown
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ConvertDataToJson(List<IExportWordCollection> exportObjects)
    {
        if (exportObjects is null)
        {
            throw new ArgumentNullException($"{nameof(exportObjects)} should not be null");
        }
        if (exportObjects.Count == 0)
        {
            throw new ArgumentException($"{nameof(exportObjects)} should not be empty");
        }
        return JsonConvert.SerializeObject(exportObjects.ToArray(), Formatting.Indented);
    }







    public async Task<ExportActionResult> ExportByCollectionOwners(List<WordCollectionOwner> owners, string path)
    {
        List<WordCollection> collections = await CollectionService.GetWordCollectionsById(owners.Select(x => x.Id).ToArray());
        return await Export(collections.ToIJsonCollection(), path);
    }




}
