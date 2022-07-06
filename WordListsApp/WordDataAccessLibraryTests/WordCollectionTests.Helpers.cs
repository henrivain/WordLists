using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary;

namespace WordDataAccessLibraryTests;

public partial class WordCollectionTests
{
    internal static readonly WordPair NormalPair = new()
    {
        NativeLanguageWord = "a Tree",
        ForeignLanguageWord = "puu",
        IndexInVocalbulary = 5,
        LearnState = WordLearnState.NeverHeard,
        OwnerId = -1
    };

    internal static readonly WordPair Pair = new()
    {
        NativeLanguageWord = null,
        ForeignLanguageWord = "puu",
        IndexInVocalbulary = 3,
        LearnState = WordLearnState.NeverHeard,
        OwnerId = -1
    };
}
