using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary;

namespace WordDataAccessLibraryTests;

public partial class WordCollectionTests
{
    [TestMethod]
    public void WordPair_HasSameWords()
    {
        string native = "a tree";
        string foreign = "puu";

        WordPair pair = new()
        {
            NativeLanguageWord = native,
            ForeignLanguageWord = foreign
        };

        Assert.AreEqual(native, pair.NativeLanguageWord);
        Assert.AreEqual(foreign, pair.ForeignLanguageWord);
    }

    [TestMethod]
    public void WordPair_NativeWordFailsOnNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            WordPair pair = new()
            {
                NativeLanguageWord = null,
                ForeignLanguageWord = "juosta"
            };
        });
    }

    [TestMethod]
    public void WordPair_ForeignWordFailsOnNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            WordPair pair = new()
            {
                NativeLanguageWord = "run",
                ForeignLanguageWord = null
            };
        });
    }

    [TestMethod]
    public void WordPair_LearnStateEnumMatchWordLearnStateId()
    {
        WordPair pair = new()
        {
            NativeLanguageWord = "a train",
            ForeignLanguageWord = "juna",
            LearnState = WordLearnState.MightKnow
        };

        Assert.AreEqual((int)WordLearnState.MightKnow, pair.WordLearnStateId);
    }

    [TestMethod]
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

        Assert.AreEqual(index, pair.IndexInVocalbulary);
    }

    [TestMethod]
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

        Assert.AreEqual(index, pair.IndexInVocalbulary);
    }

    [TestMethod]
    public void WordPair_IndexInVocalbularyThrowsException_IfSmallerThanMinusOne()
    {
        int index = -2;

        Assert.ThrowsException<ArgumentException>(() =>
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
