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

    [Theory]
    [InlineData("", "", "", 0)]
    [InlineData("MyWordCollection", "fi-en", "This is description about your collection", 0)]
    [InlineData(null, null, null, 0)]
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



    //[Theory]
    //[InlineData(false)]
    //[InlineData(true)]
    //public void StartNew_WithLengthOfZero_ShouldUpdate_Indexes(bool hasStartIndex)
    //{
    //    // Arrange
    //    var viewModel = _viewModel;
    //    WordCollection testCollection = new();

    //    // Act
    //    if (hasStartIndex)
    //        viewModel.StartNew(testCollection, 0);
    //    else
    //        viewModel.StartNew(testCollection);

    //    // Assert
    //    Assert.Equal(0, viewModel.MaxWordIndex);
    //}
    
    
    [Theory]
    [InlineData(-1, true)]
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

        // Act
        viewModel.StartNew(testCollection, startIndex);

        if (shouldThrowException) 
            Assert.Throws<ArgumentException>(action);
        else
            Assert.Equal(startIndex, viewModel.UIVisibleIndex);
    }
}
