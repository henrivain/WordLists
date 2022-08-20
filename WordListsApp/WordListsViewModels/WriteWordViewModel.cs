using System.Linq.Expressions;
using WordListsMauiHelpers.Extensions;
using WordListsViewModels.Helpers;
using WordValidationLibrary;

namespace WordListsViewModels;



[INotifyPropertyChanged]
public partial class WriteWordViewModel : IWriteWordViewModel
{
    public WriteWordViewModel(IUserInputWordValidator inputValidator)
    {
        InputValidator = inputValidator;
        StartNew(new()
        {
            Owner = new()
            {
                Name = "my collectiuon",
                LanguageHeaders = "fi-en",
                Description = "a short description about collection."
            },
            WordPairs = new()
            {
                new()
                {
                    NativeLanguageWord = "natie"
                },
                new()
                {
                    NativeLanguageWord = "asdasdsa"
                },
                new()
            }
        });
    }

    IUserInputWordValidator InputValidator { get; }

    [ObservableProperty]
    WordCollectionOwner info = new();
    

    [ObservableProperty]
    List<WordPairQuestion> questions = new();

    [ObservableProperty]
    uint questionCount = 0;

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
    });

    public void StartNew(WordCollection collection, int pairCount = -1)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));
        Info = collection.Owner;

        Questions = Enumerable.Empty<WordPairQuestion>().ToList();

        QuestionCount = (uint)collection.WordPairs.Count;

        
        
        var list = collection.WordPairs.Shuffle();
        if (pairCount > -1 && pairCount < list.Count)
        {
            list = list.GetRange(0, pairCount);
        }
        for (int i = 0; i < list.Count; i++)
        {
            Questions.Add(new(list[i], (uint)i, (uint)list.Count));
        }
    }

    
}
