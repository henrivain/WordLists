using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary;

namespace WordDataTests;
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
    public void WordPair_NativeWordFailsOnNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            WordPair pair = new()
            {
                NativeLanguageWord = null,
                ForeignLanguageWord = "juosta"
            };
        });
    }

    [Fact]
    public void WordPair_ForeignWordFailsOnNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            WordPair pair = new()
            {
                NativeLanguageWord = "run",
                ForeignLanguageWord = null
            };
        });
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
