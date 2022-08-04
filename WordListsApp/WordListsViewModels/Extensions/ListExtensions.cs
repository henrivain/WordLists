using WordListsViewModels.Helpers;

namespace WordListsViewModels.Extensions;
internal static class ListExtensions
{
    internal static List<WordCollectionOwner> SortByName(this List<WordCollectionOwner> owners)
    {
        return owners.OrderBy(x => x.Name).ToList();
    }

    internal static List<WordCollectionInfo> SortByName(this List<WordCollectionInfo> owners)
    {
        return owners.OrderBy(x => x.Owner.Name).ToList();
    }

    internal static List<WordCollectionInfo> GetOwners(this List<object> objects)
    {
        if (objects is null) return new();
        return objects.Where(x => x is WordCollectionInfo)
                      .Select(x => (WordCollectionInfo)x)
                      .ToList();
    }
}
