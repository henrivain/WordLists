using WordDataAccessLibrary;

namespace WordDataTests;
public partial class OtavaParserTests
{
    private readonly string TestDataOneLine =
   "bad hair day\r\n\r\ntosi huono päivä";

    private readonly string TestDataTwoLines =
        "bad hair day\r\n\r\ntosi huono päivä\r\n\r\nfleeting \r\n\r\nohikiitävä";

    private readonly string TestDataManyLinesPronunciation =
        "bad hair day\r\n\r\ntosi huono päivä\r\n\r\nfleeting [fliiting]\r\n\r\nohikiitävä" +
        "\r\n\r\nprincipal [prinsipl]\r\n\r\nrehtori";


    private static bool ContainsEndBracket(WordPair x)
    {
        return x.NativeLanguageWord.Contains(']') || x.ForeignLanguageWord.Contains(']');
    }

    private bool ContainsStartBracket(WordPair x)
    {
        return x.NativeLanguageWord.Contains('[') || x.ForeignLanguageWord.Contains('[');

    }
}
