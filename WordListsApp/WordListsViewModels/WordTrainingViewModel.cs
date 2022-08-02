namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordTrainingViewModel : IWordTrainingViewModel
{
    public WordTrainingViewModel()
    {
        StartNew(new());
    }

    public WordTrainingViewModel(WordCollection collection)
    {
        StartNew(collection);
    }

    public WordCollection WordCollection { get; set; } = new();

    [ObservableProperty]
    string title = "Unter uns 6 kpl 5 nice!";

    [ObservableProperty]
    string description = "This short story takes 200 symbols. " +
        "Once there was a big and massive house. " +
        "Young deer wondered what was happening inside. " +
        "It took a look insede and saw a woman with gun. " +
        "Deer got shot and died sad..";

    [ObservableProperty]
    string languageHeaders = "fi-en-sw-ge-fif";

    [ObservableProperty]
    WordPair? visibleWordPair;

    [ObservableProperty]
    int maxWordIndex = 0;

    [ObservableProperty]
    int wordIndex = 0;

    [ObservableProperty]
    bool canGoNext = false;

    [ObservableProperty]
    bool canGoPrevious = false;

    [ObservableProperty]
    int learnStateAsInt = 0;


    private void ShowListCompleted()
    {
        if (IsNotEmptyCollection) _ = UpdateCollectionToDataBase();
        ShowingListCompleted = true;
        VisibleWordPair = CompletedView;
    }
    private void ShowCurrentWord()
    {
        ShowingListCompleted = false;
        UpdateLearnStateColor();
        VisibleWordPair = GetCurrentWordPair();
    }

    private bool ShowingListCompleted { get; set; } = false;





    public void Next()
    {
        if (IsLastPair())
        {
            ShowListCompleted();
            return;
        }
        if (CanGoNext)
        {
            IncrementIndex();
            ShowCurrentWord();
        }
    }


    private bool IsLastPair()
    {
        return WordIndex == MaxWordIndex;
    }
  

    private void IncrementIndex()
    {
        CanGoPrevious = true;
        WordIndex++;
        CanGoNext = CanMoveNext();
    }
    private void DecrementIndex()
    {
        CanGoNext = true;
        WordIndex--;
        CanGoPrevious = CanMovePrevious();
    }

    public void Previous()
    {
        if (ShowingListCompleted)
        {
            ShowCurrentWord();
            return;
        }
        if (CanGoPrevious)
        {
            DecrementIndex();
            ShowCurrentWord();
        }
    }

    public async Task StartNewAsync(int collectionId)
    {
        WordCollection collection = await WordCollectionService.GetWordCollection(collectionId);
        if (collection is null)
        {
            StartNew(new WordCollection()
            {
                WordPairs = new()
                {
                    new WordPair()
                    {
                        ForeignLanguageWord = "Collection not found",
                        NativeLanguageWord = "Collection not found"
                    }
                }
            });
            return;
        }
        StartNew(collection);       
    }

    public void StartNew(WordCollection collection)
    {
        MaxWordIndex = collection.WordPairs.Count;
        StartNewWithIndex(collection, 1);
    }
    
    public void StartNew(WordCollection collection, int startIndex)
    {
        MaxWordIndex = collection.WordPairs.Count;

        startIndex = Math.Max(startIndex, 1);
        if (startIndex > MaxWordIndex)
        {
            throw new ArgumentException(
                $"{nameof(startIndex)} can't be bigger than max index {MaxWordIndex}, or smaller than 0. Was given: {startIndex}");
        }
        StartNewWithIndex(collection, startIndex);
    }



    private bool IsNotEmptyCollection { get; set; } = true;

    public IRelayCommand WordLearnedCommand => new RelayCommand(() =>
    {
        SetLearnState(WordLearnState.Learned);
    });

    public IRelayCommand MightKnowWordCommand => new RelayCommand(() =>
    {
        SetLearnState(WordLearnState.MightKnow);
    });

    public IRelayCommand WordNeverHeardCommand => new RelayCommand(() =>
    {
        SetLearnState(WordLearnState.NeverHeard);   
    });

    public IRelayCommand WordStateNotSetCommand => new RelayCommand(() =>
    {
        SetLearnState(WordLearnState.NotSet);
    });

    public IAsyncRelayCommand SaveProgression => new AsyncRelayCommand(async () =>
    {
        if (IsNotEmptyCollection)
        {
            await UpdateCollectionToDataBase();
            CollectionUpdated?.Invoke(this, new("Updated collection to match progression", WordCollection.Owner.Id));
        }
    });

    
    
    
    public event CollectionUpdatedEventHandler? CollectionUpdated;




    private void SetLearnState(WordLearnState state)
    {
        if (IsOverMaxIndex() is false)
        {
            GetCurrentWordPair().LearnState = state;
            Next();
            return;
        }
    }

    private void StartNewWithIndex(WordCollection collection, int startIndex)
    {
        IsNotEmptyCollection = true;
        WordCollection = collection;
        
        Title = collection.Owner.Name;
        Description = collection.Owner.Description;
        LanguageHeaders = collection.Owner.LanguageHeaders;

        WordIndex = startIndex;


        if (MaxWordIndex <= 0)
        {
            SetCollectionIsEmpty();
            return;
        }
        CanGoNext = CanMoveNext();
        CanGoPrevious = CanMovePrevious();
        ShowCurrentWord();
    }
    private bool CanMoveNext()
    {
        return WordIndex <= MaxWordIndex && IsNotEmptyCollection;
    }
    private bool CanMovePrevious()
    {
        return IsNotFirstWordPair() && IsNotEmptyCollection;
    }
    private void SetCollectionIsEmpty()
    {
        CanGoNext = false;
        CanGoPrevious = false;
        IsNotEmptyCollection = false;
        ShowListCompleted();
    }
   
    private void UpdateLearnStateColor()
    {
        if (IsOverMaxIndex() || VisibleWordPair is null)
        {
            LearnStateAsInt = int.MaxValue;
            return;
        }
        LearnStateAsInt = (int)VisibleWordPair!.LearnState;
    }
    private WordPair GetCurrentWordPair()
    {
        return WordCollection.WordPairs[WordIndex - 1];
    }
   
    private bool IsNotFirstWordPair()
    {
        if (WordIndex <= 1)
        {
            WordIndex = 1;
            return false;
        }
        return true;
    }
    private bool IsOverMaxIndex()
    {
        return WordIndex > MaxWordIndex;
    }
    
    private async Task UpdateCollectionToDataBase()
    {
        await WordCollectionService.SaveProgression(WordCollection);
    }

    private readonly WordPair CompletedView = new()
    {
        NativeLanguageWord = "Sanasto suoritettu!",
        ForeignLanguageWord = "Sanasto suoritettu!"
    };

}
