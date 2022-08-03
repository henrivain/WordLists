using WordDataAccessLibrary;
using WordDataAccessLibrary.Generators;

namespace WordDataTests.ParserTests;

public partial class OtavaParserTests
{
    [Fact]
    public void OneLineParsing()
    {
        OtavaWordPairParser parser = new(TestDataOneLine);

        List<WordPair> wordPairs = parser.GetList();

        Assert.Single(wordPairs);
    }

    [Fact]
    public void TwoLineParsing()
    {
        OtavaWordPairParser parser = new(TestDataTwoLines);

        List<WordPair> wordPairs = parser.GetList();

        Assert.Equal(2, wordPairs.Count);
    }

    [Fact]
    public void ThreeLinesParsing()
    {
        OtavaWordPairParser parser = new(TestDataManyLinesPronunciation);

        List<WordPair> wordPairs = parser.GetList();

        Assert.Equal(3, wordPairs.Count);
    }

    [Fact]
    public void DoesNotContainPronunciation()
    {
        OtavaWordPairParser parser = new(TestDataManyLinesPronunciation);

        List<WordPair> wordPairs = parser.GetList();

        Assert.DoesNotContain(wordPairs, x => ContainsStartBracket(x));
        Assert.DoesNotContain(wordPairs, x => ContainsEndBracket(x));
    }

    [Fact]
    public void OutputMatches_UsingOneLine()
    {
        OtavaWordPairParser parser = new(TestDataOneLine);

        List<WordPair> wordPairs = parser.GetList();

        Assert.Equal("bad hair day", wordPairs[0].NativeLanguageWord);
        Assert.Equal("tosi huono p�iv�", wordPairs[0].ForeignLanguageWord);
    }


    [Fact]
    public void OutputMatches_UsingThreeLinesAndPronunciation()
    {
        OtavaWordPairParser parser = new(TestDataManyLinesPronunciation);

        List<WordPair> wordPairs = parser.GetList();

        Assert.Equal("bad hair day", wordPairs[0].NativeLanguageWord);
        Assert.Equal("tosi huono p�iv�", wordPairs[0].ForeignLanguageWord);
        Assert.Equal("fleeting", wordPairs[1].NativeLanguageWord);
        Assert.Equal("ohikiit�v�", wordPairs[1].ForeignLanguageWord);
        Assert.Equal("principal", wordPairs[2].NativeLanguageWord);
        Assert.Equal("rehtori", wordPairs[2].ForeignLanguageWord);
    }

    [Fact]
    public void Empty()
    {
        OtavaWordPairParser parser = new(string.Empty);

        List<WordPair> wordPairs = parser.GetList();

        Assert.Empty(wordPairs);
    }

    [Fact]
    public void Null()
    {
        OtavaWordPairParser parser = new(null);
        List<WordPair> wordPairs = parser.GetList();
        Assert.Empty(wordPairs);
    }

    [Fact]
    public void OneLine()
    {
        OtavaWordPairParser parser = new("this is one line");

        List<WordPair> wordPairs = parser.GetList();

        Assert.Empty(wordPairs);
    }

    [Fact]
    public void OddNumberOfLines()
    {
        OtavaWordPairParser parser = new("1\n2\r3");

        List<WordPair> wordPairs = parser.GetList();

        Assert.Single(wordPairs);
    }

    [Fact]
    public void LineIndexing()
    {
        OtavaWordPairParser parser = new(TestDataManyLinesPronunciation);

        List<WordPair> wordPairs = parser.GetList();

        Assert.Equal(0, wordPairs[0].IndexInVocalbulary);
        Assert.Equal(1, wordPairs[1].IndexInVocalbulary);
        Assert.Equal(2, wordPairs[2].IndexInVocalbulary);
    }


    [Fact]
    public void BiggerDataSet()
    {
        string data = "end up\r\n\r\np��ty�\r\n\r\nemergency room [im�rdensi rum]" +
            "\r\n\r\np�ivystysvastaanotto\r\n\r\nget something through\r\n\r\nsaada ymm�rt�m��n" +
            "\r\n\r\nschool property [properti]\r\n\r\nkoulun tilat\r\n\r\nstrictly [striktili]" +
            "\r\n\r\nankarasti\r\n\r\nyawn [joon]\r\n\r\nhaukotella\r\n\r\nfarther [faat�]" +
            "\r\n\r\npidemm�lle\r\n\r\nreveal [riviil]\r\n\r\npaljastaa\r\n\r\nmessy\r\n\r\n" +
            "sotkuinen\r\n\r\nrude\r\n\r\nep�kohtelias\r\n";

        OtavaWordPairParser parser = new(data);

        List<WordPair> wordPairs = parser.GetList();

        Assert.Equal(10, wordPairs.Count);
    }
}