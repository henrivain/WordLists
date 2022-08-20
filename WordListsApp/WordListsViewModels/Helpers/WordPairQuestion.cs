using WordValidationLibrary;

namespace WordListsViewModels.Helpers;

public class WordPairQuestion
{
    public WordPairQuestion(WordPair wordPair, uint index = 0, uint totalIndexes = 0)
    {
        WordPair = wordPair;
        Index = index;
        TotalIndexes = totalIndexes;
    }

    public WordPair WordPair { get; set; }
    public uint Index { get; set; }
    public uint TotalIndexes { get; set; }
    public string UserAnswer { get; set; } = string.Empty;

    public WordMatchResult? MatchResult { get; set; }
}