using WordDataAccessLibrary;

namespace WordListsViewModels.Helpers;
public struct WordCollectionInfo
{
    public WordCollectionInfo(WordCollectionOwner owner, int wordPairsCount)
    {
        Owner = owner;
        WordPairsCount = wordPairsCount;
    }
    public WordCollectionOwner Owner { get; set; }
    public int WordPairsCount { get; }
}
