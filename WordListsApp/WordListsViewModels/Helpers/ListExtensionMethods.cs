using WordDataAccessLibrary;

namespace WordListsViewModels.Helpers;
internal static class ListExtensionMethods
{
    internal static List<WordCollectionOwner> SortByName(this List<WordCollectionOwner> owners)
    {
        return owners.OrderBy(x => x.Name).ToList();
    }

    internal static List<WordCollectionInfo> SortByName(this List<WordCollectionInfo> owners)
    {
        return owners.OrderBy(x => x.Owner.Name).ToList();
    }
}
