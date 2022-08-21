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
        StartNew(new());
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

    public void StartNew(WordCollection collection)
    {
        if (collection is null) throw new ArgumentNullException(nameof(collection));



        
        
      


        Info = collection.Owner;
        Questions = Enumerable.Empty<WordPairQuestion>().ToList();
        for (int i = 0; i < collection.WordPairs.Count; i++)
        {
            Questions.Add(new(collection.WordPairs[i], (uint)i+1, (uint)collection.WordPairs.Count));
        }
        QuestionCount = (uint)collection.WordPairs.Count;

    }


}
