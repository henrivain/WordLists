using static WordDataAccessLibrary.TestData.WordCollectionTestData;

namespace ViewModelTests;

public class WordTrainingViewModelTests
{
    readonly IWordTrainingViewModel _viewModel = new WordTrainingViewModel();

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
            WordPairs = new(new WordPair[1]),
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
            WordPairs = new(new WordPair[3])
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
            WordPairs = new(new WordPair[3])
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
            WordPairs = new(new WordPair[wordPairListLength])
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
            WordPairs = new(new WordPair[wordPairListLength])
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
            WordPairs = new(new WordPair[wordPairListLength])
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
            WordPairs = new(new WordPair[2])
        };

        // Act
        viewModel.StartNew(collection, 2);
        viewModel.Next();
        viewModel.Previous();
        viewModel.Previous();

        // Assert
        Assert.Equal(1, viewModel.WordIndex);
    }



}
