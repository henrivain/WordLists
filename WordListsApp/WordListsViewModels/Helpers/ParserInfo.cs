using WordDataAccessLibrary.Generators;

namespace WordListsViewModels.Helpers;
public readonly struct ParserInfo
{
    public required string Name { get; init; }
    public required IWordPairParser Parser { get; init; }
}
