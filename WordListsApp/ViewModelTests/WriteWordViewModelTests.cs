using WordDataAccessLibrary.DataBaseActions;
using WordValidationLibrary;

namespace ViewModelTests;
public class WriteWordViewModelTests
{
    readonly WriteWordViewModel _viewModel = new(new WordCollectionService(null, null), new UserInputWordValidator());

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(10, 10)]
    public void StartNew_ShouldUpdate_UserAnswers_And_ValidationResults_Length_ToMatch_MaxWordIndex(
        int wordPairListLength,
        int matchingAmount)
    {
        // Arrange
        var viewModel = _viewModel;

        WordCollection collection = new()
        {
            WordPairs = new(WordTrainingViewModelTests.GetWordPairArray(wordPairListLength))
        };
        // Act
        viewModel.StartNew(collection);

        // Assert
        Assert.Equal(matchingAmount, viewModel.UserAnswers.Length);
        Assert.Equal(matchingAmount, viewModel.MaxWordIndex);
        Assert.Equal(matchingAmount, viewModel.ValidationResults.Length);
    }
}
