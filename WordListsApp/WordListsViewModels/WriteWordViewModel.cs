using System.Reflection;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;
using WordValidationLibrary;

namespace WordListsViewModels;



[INotifyPropertyChanged]
public partial class WriteWordViewModel : IWriteWordViewModel
{
    public WriteWordViewModel(IUserInputWordValidator inputValidator)
    {
        InputValidator = inputValidator;
        StartNew(new());
    }

    IUserInputWordValidator InputValidator { get; }

    [ObservableProperty]
    WordCollectionOwner info = new();
    

    [ObservableProperty]
    List<WordPairQuestion> questions = Enumerable.Empty<WordPairQuestion>().ToList();

    [ObservableProperty]
    uint questionCount = 0;

    [ObservableProperty]
    string sessionId = GenerateSessionId();

    public IRelayCommand ValidateAll => new RelayCommand(() =>
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
        TestValidated?.Invoke(this, new()
        {
            Questions = questions,
            SessionId = SessionId
        });
    });

    public event TestValidatedEventHandler? TestValidated;

    public void StartNew(WordCollection collection)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        Info = collection.Owner;
        uint count = (uint)collection.WordPairs.Count;
        QuestionCount = count;

        // convert word pairs to word pair questions
        List<WordPairQuestion> questions = Enumerable.Empty<WordPairQuestion>().ToList();
        for (int i = 0; i < count; i++)
        {
            questions.Add(new(collection.WordPairs[i], (uint)i+1, count));
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
