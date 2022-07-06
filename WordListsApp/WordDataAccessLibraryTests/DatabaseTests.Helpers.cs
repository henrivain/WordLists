using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;

namespace WordDataAccessLibraryTests;

public partial class DatabaseTests
{
    /// <summary>
    /// All owner ids match by default
    /// </summary>
    private static readonly WordCollection WordCollection_NormalIds = new()
    {
        Owner = new()
        {
            Name = "WordCollection1",
            Description = "this is WordCollection1",
            Id = 1
        },
        WordPairs = new()
        {
            new()
            {
                NativeLanguageWord = "a Tree",
                ForeignLanguageWord = "puu",
                IndexInVocalbulary = 0,
                LearnState = WordLearnState.MightKnow,
                OwnerId = 1
            },
            new()
            {
                NativeLanguageWord = "a flower",
                ForeignLanguageWord = "kukka",
                IndexInVocalbulary = 1,
                LearnState = WordLearnState.NeverHeard,
                OwnerId = 1
            },
            new()
            {
                NativeLanguageWord = "a pig",
                ForeignLanguageWord = "sika",
                IndexInVocalbulary = 2,
                LearnState = WordLearnState.Learned,
                OwnerId = 1
            },
            new()
            {
                NativeLanguageWord = "a car",
                ForeignLanguageWord = "auto",
                IndexInVocalbulary = 3,
                LearnState = WordLearnState.NeverHeard,
                OwnerId = 1
            }

        }
    };

    /// <summary>
    /// Owner ids are random
    /// </summary>
    private static readonly WordCollection WordCollection_RandomIds = new()
    {
        Owner = new()
        {
            Name = "WordCollection2",
            Description = "this is WordCollection2",
            Id = 13
        },
        WordPairs = new()
        {
            new()
            {
                NativeLanguageWord = "a Tree",
                ForeignLanguageWord = "puu",
                IndexInVocalbulary = 0,
                OwnerId = 69
            },
            new()
            {
                NativeLanguageWord = "a flower",
                ForeignLanguageWord = "kukka",
                IndexInVocalbulary = 1,
                OwnerId = 4
            },
            new()
            {
                NativeLanguageWord = "a pig",
                ForeignLanguageWord = "sika",
                IndexInVocalbulary = 2,
                OwnerId = 2
            },
            new()
            {
                NativeLanguageWord = "a car",
                ForeignLanguageWord = "auto",
                IndexInVocalbulary = 3,
                OwnerId = 0
            }

        }
    };
}