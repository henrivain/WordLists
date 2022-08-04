using WordListsViewModels.Extensions;

namespace WordListsViewModels.Helpers;
internal static class WordCollectionInfoService
{
    internal static async Task<List<WordCollectionInfo>> GetAll()
    {
        List<WordCollection> collections = await WordCollectionService.GetWordCollections();

        return collections.Select(x => new WordCollectionInfo(x.Owner, x.WordPairs.Count))
                          .ToList()
                          .SortByName();
    }
}
