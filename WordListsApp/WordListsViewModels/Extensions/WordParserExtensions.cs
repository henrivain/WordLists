using WordDataAccessLibrary.Generators;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Extensions;
internal static class WordParserExtensions
{
    internal static IEnumerable<ParserInfo> ToParserInfos(this IEnumerable<IWordPairParser> parsers)
    {
        return parsers.Select(x => new ParserInfo { Name = GetParserName(x), Parser = x });
    }

    private static string GetParserName(IWordPairParser parser)
    {
        return parser switch
        {
            NewOtavaWordPairParser => "Otava uusi",
            OtavaWordPairParser => "Otava vanha",
            OcrWordPairParser => "Optinen tunnistus",
            _ => parser.GetType().Name,
        };
    }
}
