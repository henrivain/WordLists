using WordDataAccessLibrary.DataBaseActions.Interfaces;
namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordTrainingViewModel : IWordTrainingViewModel
{
    public WordTrainingViewModel(IWordCollectionService collectionService)
    {
        CollectionService = collectionService;
        StartNew(new());
    }

    public WordTrainingViewModel(IWordCollectionService collectionService, WordCollection collection)
    {
        CollectionService = collectionService;
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

    [ObservableProperty]
    bool progressSaved = false;

    [ObservableProperty]
    bool isListCompleted = false;
    
    [ObservableProperty]
    bool isEmptyCollection = false;

    public event CollectionUpdatedEventHandler? CollectionUpdated;




    public void Next()
    {
        if (WordIndex == MaxWordIndex)
        {
            ShowListCompleted();
            return;
        }
        if (CanGoNext)
        {
            WordIndex++;
            ShowCurrentWord();
        }
    }
    public void Previous()
    {
        if (IsListCompleted)
        {
            ShowCurrentWord();
            return;
        }
        if (CanGoPrevious)
        {
            WordIndex--;
            ShowCurrentWord();
        }
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
    public async Task StartNewAsync(int collectionId)
    {
        WordCollection collection = await CollectionService.GetWordCollection(collectionId);
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
        if (IsEmptyCollection is false)
        {
            await UpdateCollectionToDataBase();
            CollectionUpdated?.Invoke(this, new("Updated collection to match progression", WordCollection.Owner.Id));
        }
    });

    
    public IWordCollectionService CollectionService { get; }
    public IRelayCommand RestartCommand => new RelayCommand(() =>
    {
        if (IsEmptyCollection) return;
        WordIndex = 1;
        UpdateWordPairUsingWordIndex();
    });

    private void StartNewWithIndex(WordCollection collection, int startIndex)
    {
        ProgressSaved = true;
        WordCollection = collection;
        SetCollectionInfo();
        WordIndex = startIndex;
        UpdateWordPairUsingWordIndex();
    }

    private void UpdateWordPairUsingWordIndex()
    {
        if (MaxWordIndex <= 0)
        {
            ShowCollectionIsEmpty();
            return;
        }
        IsEmptyCollection = false;
        ShowCurrentWord();
    }
    private void SetCollectionInfo()
    {
        Title = WordCollection.Owner.Name;
        Description = WordCollection.Owner.Description;
        LanguageHeaders = WordCollection.Owner.LanguageHeaders;
    }
    private void ShowCurrentWord()
    {
        IsListCompleted = false;

        if (WordIndex <= 1) WordIndex = 1;
        CanGoPrevious = IsEmptyCollection is false && WordIndex > 1;
        CanGoNext = (IsEmptyCollection || IsListCompleted) is false;

        UpdateFlipCardColor();
        VisibleWordPair = WordCollection.WordPairs[WordIndex - 1];
    }
    private void ShowListCompleted()
    {
        IsListCompleted = true;
        if (IsEmptyCollection is false) _ = UpdateCollectionToDataBase();
        CanGoNext = false;
        VisibleWordPair = CompletedView;
    }
    private void ShowCollectionIsEmpty()
    {
        MaxWordIndex = 1;
        CanGoNext = false;
        CanGoPrevious = false;
        IsEmptyCollection = true;
        ShowListCompleted();
    }


    private void UpdateFlipCardColor()
    {
        if (IsListCompleted || VisibleWordPair is null)
        {
            LearnStateAsInt = int.MaxValue;
            return;
        }
        LearnStateAsInt = (int)VisibleWordPair!.LearnState;
    }
    private void SetLearnState(WordLearnState state)
    {
        if (IsListCompleted is false && VisibleWordPair is not null)
        {
            ProgressSaved = false;
            VisibleWordPair.LearnState = state;
            Next();
            return;
        }
    }
    private async Task UpdateCollectionToDataBase()
    {
        if (IsEmptyCollection) return;
        await CollectionService.SaveProgression(WordCollection);
        ProgressSaved = true;
    }

    private readonly WordPair CompletedView = new()
    {
        NativeLanguageWord = "Sanasto suoritettu!",
        ForeignLanguageWord = "Sanasto suoritettu!"
    };

}
