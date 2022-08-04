using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.Extensions;

namespace WordDataAccessLibrary.JsonServices;
public static class JsonExportService
{
    public static async Task<JsonActionArgs> ExportJson(List<IExportWordCollection> exportCollections, string path)
    {
        path = path.Trim();
        if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
        if (path.EndsWith(".json") is false) path += ".json";

        await Task.Delay(1);
        throw new NotImplementedException();
    }

    public static async Task<JsonActionArgs> ExportByOwners(List<WordCollectionOwner> owners, string path)
    {
        List<WordCollection> collections = await WordCollectionService.GetWordCollectionsById(owners.Select(x => x.Id).ToArray());
        return await ExportJson(collections.ToIJsonCollection(), path);
    }

    
}
