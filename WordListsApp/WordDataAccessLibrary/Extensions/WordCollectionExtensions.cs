using WordDataAccessLibrary.ExportServices;

namespace WordDataAccessLibrary.Extensions;
internal static class WordCollectionExtensions
{
    internal static List<IExportWordCollection> ToIJsonCollection(this List<WordCollection> collections)
    {
        return collections
            .Select(c => new ExportWordCollection().FromWordCollection(c))
            .Where(ec => ec is not null)
            .ToList();
    }
}
