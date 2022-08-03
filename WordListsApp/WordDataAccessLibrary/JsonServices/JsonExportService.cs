using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary.DataBaseActions;

namespace WordDataAccessLibrary.JsonServices;
public class JsonExportService
{
    //public async Task<JsonActionResult> ExportAsync()
    //{
    //    throw new NotImplementedException();
    //    //return new(JsonAction.NotDefined);
    //}
    public static async Task ExportJson(List<IJsonWordCollection> exportCollections, string path)
    {
        path = path.Trim();
        if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
        if (path.EndsWith(".json") is false) path += ".json";



        throw new NotImplementedException();
    }

    public static async Task ExportCollections(List<WordCollectionOwner> owners)
    {
        List<IJsonWordCollection> exportCollections = new();

        foreach (WordCollectionOwner owner in owners)
        {
            WordCollection collection = await WordCollectionService.GetWordCollection(owner.Id);
            ExportWordCollection jsonCollection = new();
            if (jsonCollection.FromWordCollection(collection) is null) continue;
            exportCollections.Add(jsonCollection);
        }

        await ExportJson(exportCollections, null);
    }
}
