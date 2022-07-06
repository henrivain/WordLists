using WordDataAccessLibrary.Generators;
using WordDataAccessLibrary;

namespace WordDataAccessLibraryTests;

[TestClass]
public partial class OtavaParserTests
{
      [TestMethod]
    public void OneLineParsing()
    {
        OtavaWordPairParser parser = new(TestDataOneLine);

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual(1, wordPairs.Count);
    }

    [TestMethod]
    public void TwoLineParsing()
    {
        OtavaWordPairParser parser = new(TestDataTwoLines);

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual(2, wordPairs.Count);
    }

    [TestMethod]
    public void ThreeLinesParsing()
    {
        OtavaWordPairParser parser = new(TestDataManyLinesPronunciation);

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual(3, wordPairs.Count);
    }

    [TestMethod]
    public void DoesNotContainPronunciation()
    {
        OtavaWordPairParser parser = new(TestDataManyLinesPronunciation);

        List<WordPair> wordPairs = parser.GetList();

        Assert.IsFalse(wordPairs.Any(x => ContainsStartBracket(x)));
        Assert.IsFalse(wordPairs.Any(x => ContainsEndBracket(x)));
    }

    [TestMethod]
    public void OutputMatches_UsingOneLine()
    {
        OtavaWordPairParser parser = new(TestDataOneLine);

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual("bad hair day", wordPairs[0].ForeignLanguageWord);
        Assert.AreEqual("tosi huono päivä", wordPairs[0].NativeLanguageWord);
    }
    
    
    [TestMethod]
    public void OutputMatches_UsingThreeLinesAndPronunciation()
    {
        OtavaWordPairParser parser = new(TestDataManyLinesPronunciation);

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual("bad hair day", wordPairs[0].ForeignLanguageWord);
        Assert.AreEqual("tosi huono päivä", wordPairs[0].NativeLanguageWord);
        Assert.AreEqual("fleeting", wordPairs[1].ForeignLanguageWord);
        Assert.AreEqual("ohikiitävä", wordPairs[1].NativeLanguageWord);
        Assert.AreEqual("principal", wordPairs[2].ForeignLanguageWord);
        Assert.AreEqual("rehtori", wordPairs[2].NativeLanguageWord);
    }
    
    [TestMethod]
    public void Empty()
    {
        OtavaWordPairParser parser = new(string.Empty);

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual(0, wordPairs.Count);
    }
    
    [TestMethod]
    public void Null()
    {
        OtavaWordPairParser parser = new(null);

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual(0, wordPairs.Count);
    }

    [TestMethod]
    public void OneLine()
    {
        OtavaWordPairParser parser = new("this is one line");

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual(0, wordPairs.Count);
    }
    
    [TestMethod]
    public void OddNumberOfLines()
    {
        OtavaWordPairParser parser = new("1\n2\r3");

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual(1, wordPairs.Count);
    }

    [TestMethod]
    public void LineIndexing()
    {
        OtavaWordPairParser parser = new(TestDataManyLinesPronunciation);

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual((uint)0, wordPairs[0].IndexInVocalbulary);
        Assert.AreEqual((uint)1, wordPairs[1].IndexInVocalbulary);
        Assert.AreEqual((uint)2, wordPairs[2].IndexInVocalbulary);
    }


    [TestMethod]
    public void BiggerDataSet()
    {
        string data = "end up\r\n\r\npäätyä\r\n\r\nemergency room [imördensi rum]" +
            "\r\n\r\npäivystysvastaanotto\r\n\r\nget something through\r\n\r\nsaada ymmärtämään" +
            "\r\n\r\nschool property [properti]\r\n\r\nkoulun tilat\r\n\r\nstrictly [striktili]" +
            "\r\n\r\nankarasti\r\n\r\nyawn [joon]\r\n\r\nhaukotella\r\n\r\nfarther [faatö]" +
            "\r\n\r\npidemmälle\r\n\r\nreveal [riviil]\r\n\r\npaljastaa\r\n\r\nmessy\r\n\r\n" +
            "sotkuinen\r\n\r\nrude\r\n\r\nepäkohtelias\r\n";

        OtavaWordPairParser parser = new(data);

        List<WordPair> wordPairs = parser.GetList();

        Assert.AreEqual(10, wordPairs.Count);
    }
}