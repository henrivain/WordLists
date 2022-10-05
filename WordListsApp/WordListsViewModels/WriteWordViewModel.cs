using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsViewModels.Events;
using WordListsViewModels.Extensions;
using WordListsViewModels.Helpers;
using WordValidationLibrary;

namespace WordListsViewModels;



[INotifyPropertyChanged]
public partial class WriteWordViewModel : IWriteWordViewModel
{
    public WriteWordViewModel(IUserInputWordValidator inputValidator, IWordPairService wordPairService)
    {
        InputValidator = inputValidator;
        WordPairService = wordPairService;
        StartNew(new());
    }

    IUserInputWordValidator InputValidator { get; }
    IWordPairService WordPairService { get; }

    [ObservableProperty]
    WordCollectionOwner info = new();


    [ObservableProperty]
    List<WordPairQuestion> questions = Enumerable.Empty<WordPairQuestion>().ToList();

    [ObservableProperty]
    uint questionCount = 0;

    [ObservableProperty]
    string sessionId = GenerateSessionId();

    [ObservableProperty]
    bool saveProgression = false;

    public IAsyncRelayCommand ValidateAll => new AsyncRelayCommand(async () =>
    {
        foreach (var question in Questions)
        {
            question.MatchResult = InputValidator.CompareWords(question.UserAnswer, question.WordPair.ForeignLanguageWord);
            switch (question.MatchResult.CharMatchPercentage)
            {
                case >= 100:
                    question.WordPair.LearnState = WordLearnState.Learned;
                    break;
                case >= 70:
                    question.WordPair.LearnState = WordLearnState.MightKnow;
                    break;
                case <= 20:
                    question.WordPair.LearnState = WordLearnState.NeverHeard;
                    break;
            }
        }
        if (SaveProgression)
        {
            await questions.SaveLearnStates(WordPairService);
        }
        TestValidated?.Invoke(this, new()
        {
            Questions = questions,
            SessionId = SessionId,
            ProgressionSaved = SaveProgression
        });
    });

    public event TestValidatedEventHandler? TestValidated;

    public void StartNew(WordCollection collection)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        SessionId = GenerateSessionId();
        Info = collection.Owner;
        uint count = (uint)collection.WordPairs.Count;
        QuestionCount = count;

        // convert word pairs to word pair questions
        List<WordPairQuestion> questions = Enumerable.Empty<WordPairQuestion>().ToList();
        for (int i = 0; i < count; i++)
        {
            questions.Add(new(collection.WordPairs[i], (uint)i + 1, count));
        }
        Questions = questions;
    }

    /// <summary>
    /// Generate session id from current time hash code
    /// </summary>
    /// <returns>id starting with '#' followed by 0-7 numbers</returns>
    private static string GenerateSessionId()
    {
        string sessionId = DateTime.Now.GetHashCode().ToString().Replace('-', '1');
        try
        {
            return $"#{sessionId[0..7]}";
        }
        catch (ArgumentOutOfRangeException)
        {
            return $"#{sessionId}";
        }
    }
}
