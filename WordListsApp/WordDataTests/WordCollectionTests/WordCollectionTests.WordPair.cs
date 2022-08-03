using WordDataAccessLibrary;

namespace WordDataTests.ParserTests;
public partial class WordCollectionTests
{
    [Fact]
    public void WordPair_HasSameWords()
    {
        string native = "a tree";
        string foreign = "puu";

        WordPair pair = new()
        {
            NativeLanguageWord = native,
            ForeignLanguageWord = foreign
        };

        Assert.Equal(native, pair.NativeLanguageWord);
        Assert.Equal(foreign, pair.ForeignLanguageWord);
    }

    [Fact]
    public void WordPair_NativeWordNull_ShouldChangeTo_EmptyString()
    {
        WordPair pair = new()
        {
            NativeLanguageWord = null,
            ForeignLanguageWord = "juosta"
        };
        Assert.Equal(string.Empty, pair.NativeLanguageWord);

    }

    [Fact]
    public void WordPair_ForeignWordNull_ShouldChangeTo_EmptyString()
    {
        WordPair pair = new()
        {
            NativeLanguageWord = "run",
            ForeignLanguageWord = null
        };
        Assert.Equal(string.Empty, pair.ForeignLanguageWord);
    }

    [Fact]
    public void WordPair_LearnStateEnumMatchWordLearnStateId()
    {
        WordPair pair = new()
        {
            NativeLanguageWord = "a train",
            ForeignLanguageWord = "juna",
            LearnState = WordLearnState.MightKnow
        };

        Assert.Equal((int)WordLearnState.MightKnow, pair.WordLearnStateId);
    }

    [Fact]
    public void WordPair_RemembersNormalIndexInVocalbulary()
    {
        int index = 3;

        WordPair pair = new()
        {
            NativeLanguageWord = "a word",
            ForeignLanguageWord = "sana",
            LearnState = WordLearnState.Learned,
            IndexInVocalbulary = index
        };

        Assert.Equal(index, pair.IndexInVocalbulary);
    }

    [Fact]
    public void WordPair_IndexInVocalbularyCanBeMinusOne()
    {
        int index = -1;

        WordPair pair = new()
        {
            NativeLanguageWord = "a word",
            ForeignLanguageWord = "sana",
            LearnState = WordLearnState.Learned,
            IndexInVocalbulary = index
        };

        Assert.Equal(index, pair.IndexInVocalbulary);
    }

    [Fact]
    public void WordPair_IndexInVocalbularyThrowsException_IfSmallerThanMinusOne()
    {
        int index = -2;
        
        Assert.Throws<ArgumentException>(() =>
        {
            WordPair pair = new()
            {
                NativeLanguageWord = "a word",
                ForeignLanguageWord = "sana",
                LearnState = WordLearnState.Learned,
                IndexInVocalbulary = index
            };
        });
    }
}
