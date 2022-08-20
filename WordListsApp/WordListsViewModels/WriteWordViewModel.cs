using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordValidationLibrary;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WriteWordViewModel : WordTrainingViewModel, IWriteWordViewModel
{
    public WriteWordViewModel(IWordCollectionService collectionService, IUserInputWordValidator inputValidator) : base(collectionService)
    {
        InputValidator = inputValidator;
    }
    IUserInputWordValidator InputValidator { get; }

    [ObservableProperty]
    string userInput = string.Empty;
    
    [ObservableProperty]
    string[] userAnswers = Array.Empty<string>();

    [ObservableProperty]
    WordMatchResult? validationResult;

    [ObservableProperty]
    WordMatchResult?[] validationResults = Array.Empty<WordMatchResult?>();


    public IRelayCommand ValidateWord => new RelayCommand(() =>
    {
        if (VisibleWordPair is null) return;

        WordMatchResult result = InputValidator.CompareWords(UserInput, VisibleWordPair.ForeignLanguageWord);

        switch (result.CharMatchPercentage)
        {
            case >= 100:
                VisibleWordPair.LearnState = WordLearnState.Learned;
                ProgressSaved = false;
                break;
            case > 70:
                VisibleWordPair.LearnState = WordLearnState.MightKnow;
                ProgressSaved = false;
                break;
            case < 20:
                VisibleWordPair.LearnState = WordLearnState.NeverHeard;
                ProgressSaved = false;
                break;
        }

        ValidationResults[WordIndex] = result;
    });

    public IRelayCommand GoNext => new RelayCommand(() =>
    {
        base.Next();
        UpdateValidationVisibility();
    });
    
    public IRelayCommand GoPrevious => new RelayCommand(() =>
    {
        base.Previous();
        UpdateValidationVisibility();
    });


    public override void StartNew(WordCollection collection)
    {
        base.StartNew(collection);
        ResetAnswerArrays();

    }

    public override void StartNew(WordCollection collection, int startIndex)
    {
        base.StartNew(collection, startIndex);
        ResetAnswerArrays();
    }

    private void ResetAnswerArrays()
    {
        UserAnswers = new string[MaxWordIndex];
        Array.Fill(UserAnswers, string.Empty);
        ValidationResults = new WordMatchResult[MaxWordIndex];
        Array.Fill(ValidationResults, null);
    }
    private void UpdateValidationVisibility()
    {
        if (IsListCompleted is false)
        {
            UserInput = UserAnswers[WordIndex];
            ValidationResult = ValidationResults[WordIndex];
        }
    }

}
