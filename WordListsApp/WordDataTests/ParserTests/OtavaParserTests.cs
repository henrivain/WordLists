using WordDataAccessLibrary;
using WordDataAccessLibrary.Generators;

namespace WordDataTests.ParserTests;

public partial class OtavaParserTests
{
    readonly OtavaWordPairParser _parser = new();

    [Fact]
    public void OneLineParsing()
    {
        List<WordPair> wordPairs = _parser.GetList(TestDataOneLine);
        Assert.Single(wordPairs);
    }

    [Fact]
    public void TwoLineParsing()
    {

        List<WordPair> wordPairs = _parser.GetList(TestDataTwoLines);

        Assert.Equal(2, wordPairs.Count);
    }

    [Fact]
    public void ThreeLinesParsing()
    {
        List<WordPair> wordPairs = _parser.GetList(TestDataManyLinesPronunciation);

        Assert.Equal(3, wordPairs.Count);
    }

    [Fact]
    public void DoesNotContainPronunciation()
    {
        List<WordPair> wordPairs = _parser.GetList(TestDataManyLinesPronunciation);

        Assert.DoesNotContain(wordPairs, x => ContainsStartBracket(x));
        Assert.DoesNotContain(wordPairs, x => ContainsEndBracket(x));
    }

    [Fact]
    public void OutputMatches_UsingOneLine()
    {
        List<WordPair> wordPairs = _parser.GetList(TestDataOneLine);

        Assert.Equal("bad hair day", wordPairs[0].NativeLanguageWord);
        Assert.Equal("tosi huono päivä", wordPairs[0].ForeignLanguageWord);
    }


    [Fact]
    public void OutputMatches_UsingThreeLinesAndPronunciation()
    {
        List<WordPair> wordPairs = _parser.GetList(TestDataManyLinesPronunciation);

        Assert.Equal("bad hair day", wordPairs[0].NativeLanguageWord);
        Assert.Equal("tosi huono päivä", wordPairs[0].ForeignLanguageWord);
        Assert.Equal("fleeting", wordPairs[1].NativeLanguageWord);
        Assert.Equal("ohikiitävä", wordPairs[1].ForeignLanguageWord);
        Assert.Equal("principal", wordPairs[2].NativeLanguageWord);
        Assert.Equal("rehtori", wordPairs[2].ForeignLanguageWord);
    }

    [Fact]
    public void Empty()
    {
        List<WordPair> wordPairs = _parser.GetList(string.Empty);

        Assert.Empty(wordPairs);
    }

    [Fact]
    public void Null()
    {
        List<WordPair> wordPairs = _parser.GetList(null);
        Assert.Empty(wordPairs);
    }

    [Fact]
    public void OneLine()
    {
        List<WordPair> wordPairs = _parser.GetList("this is one line");

        Assert.Empty(wordPairs);
    }

    [Fact]
    public void OddNumberOfLines()
    {
        List<WordPair> wordPairs = _parser.GetList("1\n2\r3");

        Assert.Single(wordPairs);
    }

    [Fact]
    public void LineIndexing()
    {
        List<WordPair> wordPairs = _parser.GetList(TestDataManyLinesPronunciation);

        Assert.Equal(0, wordPairs[0].IndexInVocalbulary);
        Assert.Equal(1, wordPairs[1].IndexInVocalbulary);
        Assert.Equal(2, wordPairs[2].IndexInVocalbulary);
    }


    [Fact]
    public void BiggerDataSet()
    {
        string data = "end up\r\n\r\npäätyä\r\n\r\nemergency room [imördensi rum]" +
            "\r\n\r\npäivystysvastaanotto\r\n\r\nget something through\r\n\r\nsaada ymmärtämään" +
            "\r\n\r\nschool property [properti]\r\n\r\nkoulun tilat\r\n\r\nstrictly [striktili]" +
            "\r\n\r\nankarasti\r\n\r\nyawn [joon]\r\n\r\nhaukotella\r\n\r\nfarther [faatö]" +
            "\r\n\r\npidemmälle\r\n\r\nreveal [riviil]\r\n\r\npaljastaa\r\n\r\nmessy\r\n\r\n" +
            "sotkuinen\r\n\r\nrude\r\n\r\nepäkohtelias\r\n";

        List<WordPair> wordPairs = _parser.GetList(data);

        Assert.Equal(10, wordPairs.Count);
    }
}