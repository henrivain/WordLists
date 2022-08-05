using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Extensions;

namespace WordDataAccessLibrary.JsonServices;
public class JsonExportService : IExportService
{
    public JsonExportService(IWordCollectionService collectionService)
    {
        CollectionService = collectionService;
    }

    IWordCollectionService CollectionService { get; }

    public async Task<JsonActionArgs> Export(List<IExportWordCollection> exportCollections, string path)
    {
        path = path.Trim();
        if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
        if (path.EndsWith(".json") is false) path += ".json";

        await Task.Delay(1);
        throw new NotImplementedException();
    }

    public async Task<JsonActionArgs> ExportByOwners(List<WordCollectionOwner> owners, string path)
    {
        List<WordCollection> collections = await CollectionService.GetWordCollectionsById(owners.Select(x => x.Id).ToArray());
        return await Export(collections.ToIJsonCollection(), path);
    }


}
