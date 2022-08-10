using WordDataAccessLibrary.DataBaseActions;

namespace ViewModelTests;

public partial class WordTrainingViewModelTests
{
    public WordTrainingViewModelTests()
    {
        _viewModel = new WordTrainingViewModel(new WordCollectionService(null, null));
    }

    readonly IWordTrainingViewModel _viewModel;

    [Theory]
    [InlineData("", "", "")]
    [InlineData("MyWordCollection", "fi-en", "This is description about your collection")]
    [InlineData(null, null, null)]
    public void StartNew_ShouldUpdate_TitleAndLanguagesAndDescription(
        string name,
        string langHeaders,
        string description
        )
    {
        // Arrange
        var viewModel = _viewModel;
        WordCollection testCollection = new()
        {
            Owner = new()
            {
                Name = name,
                LanguageHeaders = langHeaders,
                Description = description
            }
        };

        // Act

        viewModel.StartNew(testCollection);

        // Assert
        Assert.Equal(name, viewModel.Title);
        Assert.Equal(langHeaders, viewModel.LanguageHeaders);
        Assert.Equal(description, viewModel.Description);
    }


    [Fact]
    public void StartNew_WithEmptyCollection_WithIndexZero_ShouldThrowArgumentException()
    {
        // Arrange
        var viewModel = _viewModel;
        var action = () => viewModel.StartNew(new(), 0);

        // Act & Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData("", "", "", 1)]
    [InlineData("MyWordCollection", "fi-en", "This is description about your collection", 1)]
    [InlineData(null, null, null, 1)]
    public void StartNew_WithIndex_ShouldUpdate_TitleAndLanguagesAndDescription(
        string name,
        string langHeaders,
        string description,
        int startIndex
        )
    {
        // Arrange
        var viewModel = _viewModel;
        WordCollection testCollection = new()
        {
            WordPairs = new(GetWordPairArray(1)),
            Owner = new()
            {
                Name = name,
                LanguageHeaders = langHeaders,
                Description = description
            }
        };

        // Act

        viewModel.StartNew(testCollection, startIndex);

        // Assert
        Assert.Equal(name, viewModel.Title);
        Assert.Equal(langHeaders, viewModel.LanguageHeaders);
        Assert.Equal(description, viewModel.Description);
    }

    [Fact]
    public void StartNew_EmptyCollection_ShouldSet_MaxWordIndex_AndWordIndex_One()
    {
        // Arrange
        var viewModel = _viewModel;
        WordCollection testCollection = new()
        {
            WordPairs = new()
        };
        // Act

        viewModel.StartNew(testCollection);

        // Assert
        Assert.Equal(1, viewModel.MaxWordIndex);
        Assert.Equal(1, viewModel.WordIndex);
    }


    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(4, true)]
    public void StartNew_WithLengthOfThree_ShouldValidate_StartIndex(int startIndex, bool shouldThrowException)
    {
        // Arrange
        var viewModel = _viewModel;
        WordCollection testCollection = new()
        {
            WordPairs = new(GetWordPairArray(3))
        };
        var action = () => viewModel.StartNew(testCollection, startIndex);

        // Act & Assert
        if (shouldThrowException) 
            Assert.Throws<ArgumentException>(action);
        else
            action();
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    public void StartNew_TooSmallStartIndex_ShouldBeManipulated(int startIndex, int shouldMatchIndex)
    {
        // Arrange
        var viewModel = _viewModel;
        WordCollection testCollection = new()
        {
            WordPairs = new(GetWordPairArray(3))
        };

        // Act
        viewModel.StartNew(testCollection, startIndex);

        // Assert
        Assert.Equal(shouldMatchIndex, viewModel.WordIndex);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(2, 2)]
    [InlineData(10, 5)]
    public void StartNew_ShouldUpdate_Indexes(int wordPairListLength, int startIndex = 1)
    {
        // Arrange
        var viewModel = _viewModel;

        WordCollection collection = new()
        {
            WordPairs = new(GetWordPairArray(wordPairListLength))
        };

        // Act
        viewModel.StartNew(collection, startIndex);

        // Assert
        Assert.Equal(wordPairListLength, viewModel.MaxWordIndex);
        Assert.Equal(startIndex, viewModel.WordIndex);
    }


    [Theory]
    [InlineData(1, 1, 3, 1)]
    [InlineData(3, 1, 1, 2)]
    [InlineData(5, 2, 2, 4)]
    public void Next_ShouldSet_ValidWordIndex(
        int wordPairListLength, 
        int startIndex, 
        int callTimes, 
        int expectedIndex)
    {
        // Arrange
        var viewModel = _viewModel;
        WordCollection collection = new()
        {
            WordPairs = new(GetWordPairArray(wordPairListLength))
        };

        // Act
        viewModel.StartNew(collection, startIndex);
        for (int i = 0; i < callTimes; i++)
        {
            viewModel.Next();
        }

        // Assert
        Assert.Equal(expectedIndex, viewModel.WordIndex);
    }

    [Theory]
    [InlineData(1, 1, 3, 1)]
    [InlineData(3, 3, 1, 2)]
    [InlineData(5, 4, 2, 2)]
    public void Previous_ShouldSet_ValidWordIndex(
        int wordPairListLength, 
        int startIndex, 
        int callTimes, 
        int expectedIndex)
    {
        // Arrange
        var viewModel = _viewModel;
        WordCollection collection = new()
        {
            WordPairs = new(GetWordPairArray(wordPairListLength))
        };

        // Act
        viewModel.StartNew(collection, startIndex);
        for (int i = 0; i < callTimes; i++)
        {
            viewModel.Previous();
        }

        // Assert
        Assert.Equal(expectedIndex, viewModel.WordIndex);
    }


    [Fact]
    public void UsingNextOnLastWord_ThenPreviousTwoTimes_ShouldMatchSecondToLastWordPair()
    {
        // Arrange
        var viewModel = _viewModel;
        WordCollection collection = new()
        {
            WordPairs = new(GetWordPairArray(2))
        };

        // Act
        viewModel.StartNew(collection, 2);
        viewModel.Next();
        viewModel.Previous();
        viewModel.Previous();

        // Assert
        Assert.Equal(1, viewModel.WordIndex);
    }

    [Fact]
    public void WordStateNotSetCommand_ShouldSet_LearnStateNotSet()
    {
        // Arrange
        var viewModel = _viewModel;
        viewModel.StartNew(new()
        {
            WordPairs = new()
            {
                new()
                {
                    LearnState = WordLearnState.Learned
                }
            }
        });
        // Act
        viewModel.WordStateNotSetCommand.Execute(null);

        // Assert
        Assert.Equal(WordLearnState.NotSet, viewModel.WordCollection.WordPairs.First().LearnState);
    }

    [Fact]
    public void WordLearnedCommand_ShouldSet_LearnStateLearned()
    {
        // Arrange
        var viewModel = _viewModel;
        viewModel.StartNew(new()
        {
            WordPairs = new()
            {
                new()
                {
                    LearnState = WordLearnState.NotSet
                }
            }
        });
        // Act
        viewModel.WordLearnedCommand.Execute(null);

        // Assert
        Assert.Equal(WordLearnState.Learned, GetFirstPair(viewModel).LearnState);
    }

    [Fact]
    public void MightKnowWordCommand_ShouldSet_LearnStateLearned()
    {
        // Arrange
        var viewModel = _viewModel;
        viewModel.StartNew(new()
        {
            WordPairs = new()
            {
                new()
                {
                    LearnState = WordLearnState.NotSet
                }
            }
        });
        // Act
        viewModel.MightKnowWordCommand.Execute(null);

        // Assert
        Assert.Equal(WordLearnState.MightKnow, GetFirstPair(viewModel).LearnState);
    }

    [Fact]
    public void WordNeverHeardCommand_ShouldSet_LearnStateLearned()
    {
        // Arrange
        var viewModel = _viewModel;
        viewModel.StartNew(new()
        {
            WordPairs = new()
            {
                new()
                {
                    LearnState = WordLearnState.NotSet
                }
            }
        });
        // Act
        viewModel.WordNeverHeardCommand.Execute(null);

        // Assert
        Assert.Equal(WordLearnState.NeverHeard, GetFirstPair(viewModel).LearnState);
    }

    [Theory]
    [InlineData(2,2,0)]
    [InlineData(2,1,1)]
    [InlineData(10,1,4)]
    [InlineData(0,1,0)]

    public void VisibleWordPair_ShouldChange_WhenNextCalled(int nextClickedTimes, int previousClickedTimes, int matchingArrayIndex)
    {
        // Arrange
        var viewModel = _viewModel;
        var wordPairs = GetWordPairArray(5);

        viewModel.StartNew(new()
        {
            WordPairs = new(wordPairs)
        });

        // Act
        for (int i = 0; i < nextClickedTimes; i++)
        {
            viewModel.Next();
        }
        for (int i = 0; i < previousClickedTimes; i++)
        {
            viewModel.Previous();
        }

        // Assert
        Assert.Equal(wordPairs[matchingArrayIndex], viewModel.VisibleWordPair);
    }

    [Fact]
    public void ShowingListCompleted_ShouldUpdate_CanGoNext()
    {
        // Arrange
        var viewModel = _viewModel;
        viewModel.StartNew(new()
        {
            WordPairs = new(GetWordPairArray(1))
        });

        // Act
        viewModel.Next();

        // Assert
        Assert.False(viewModel.CanGoNext);
    }

    [Fact]
    public void ShowingFirstPair_ShouldUpdate_CanGoPrevious()
    {
        // Arrange
        var viewModel = _viewModel;

        // Act
        viewModel.StartNew(new()
        {
            WordPairs = new(GetWordPairArray(1))
        });

        // Assert
        Assert.False(viewModel.CanGoPrevious);
    }
    
    [Fact]
    public void StartingEmpty_ShouldSet_CanGoPreviousAndCanGoNext_False()
    {
        // Arrange
        var viewModel = _viewModel;

        // Act
        viewModel.StartNew(new());

        // Assert
        Assert.False(viewModel.CanGoPrevious);
        Assert.False(viewModel.CanGoNext);
    }


    [Fact]
    public void UsingPrevious_InCompletedView_ShouldUpdate_CanGoNext()
    {
        // Arrange
        var viewModel = _viewModel;
        viewModel.StartNew(new()
        {
            WordPairs = new(GetWordPairArray(2))
        });

        // Act
        viewModel.Next();
        viewModel.Next();
        viewModel.Previous();
        

        // Assert
        Assert.True(viewModel.CanGoNext);
    }

    [Fact]
    public void StartNew_WithEmptyCollection_ShouldSet_IsEmptyCollectio_True()
    {
        // Arrange
        var viewModel = _viewModel;
        // Act
        viewModel.StartNew(new());
        //Assert
        Assert.True(viewModel.IsEmptyCollection);
    }
}
