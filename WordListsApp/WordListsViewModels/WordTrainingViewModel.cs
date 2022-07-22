using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using static WordDataAccessLibrary.DataBaseActions.DataBaseDelegates;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordTrainingViewModel : IWordTrainingViewModel
{
    public WordTrainingViewModel()
    {
        StartNew(new());
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
    public int realIndex = 0;

    [ObservableProperty]
    bool canGoNext = false;

    [ObservableProperty]
    bool canGoPrevious = false;

    [ObservableProperty]
    int uIVisibleIndex = 0;

    [ObservableProperty]
    int learnStateAsInt = 0;

    public IAsyncRelayCommand SaveProgression => new AsyncRelayCommand(async () =>
    {
        if (IsNotEmptyCollection)
        {
            await UpdateCollectionToDataBase();
            CollectionUpdated?.Invoke(this, new("Updated collection to match progression", WordCollection.Owner.Id));
        }
    });

    public event CollectionUpdatedEventHandler? CollectionUpdated;

    public void Next()
    {
        if (CanMoveNext())
        {
            CanGoPrevious = true;
            RealIndex++;
            CanGoNext = CanMoveNext();
            UIVisibleIndex = GetUIIndex();
            ShowCurrentWord();
        }
    }
    public void Previous()
    {
        if (CanGoPrevious)
        {
            CanGoNext = true;
            RealIndex--;
            CanGoPrevious = CanMovePrevious();
            UIVisibleIndex = GetUIIndex();
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
        MaxWordIndex = collection.WordPairs.Count - 1;
        StartNewWithIndex(collection, 0);
    }

    public void StartNew(WordCollection collection, int fromIndex)
    {
        MaxWordIndex = collection.WordPairs.Count - 1;

        if (StartIndexNotValid(fromIndex))
        {
            throw new ArgumentException(
                $"{nameof(fromIndex)} can't be bigger than max index, or smaller than 0. Was given: {fromIndex}, max: {MaxWordIndex}");
        }
        StartNewWithIndex(collection, fromIndex);
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

        RealIndex = startIndex;
        if (MaxWordIndex < 0)
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
        return RealIndex <= MaxWordIndex && IsNotEmptyCollection;
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
        ShowCurrentWord();
        MaxWordIndex = 0;
    }
    private void ShowCurrentWord()
    {
        UpdateLearnStateColor();
        if (IsOverMaxIndex())
        {
            if (IsNotEmptyCollection) _ = UpdateCollectionToDataBase();
            VisibleWordPair = CompletedView;
            return;
        }
        VisibleWordPair = GetCurrentWordPair();
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
        return WordCollection.WordPairs[UIVisibleIndex];
    }
    private int GetUIIndex()
    {
        return (RealIndex > MaxWordIndex) ? MaxWordIndex : RealIndex;
    }
    private bool IsNotFirstWordPair()
    {
        if (RealIndex <= 0)
        {
            RealIndex = 0;
            return false;
        }
        return true;
    }
    private bool IsOverMaxIndex()
    {
        return RealIndex > MaxWordIndex;
    }
    private bool StartIndexNotValid(int fromIndex)
    {
        return fromIndex > MaxWordIndex || fromIndex < 0;
    }
    private async Task UpdateCollectionToDataBase()
    {
        await WordCollectionService.SaveProgression(WordCollection);
    }

    private readonly WordPair CompletedView = new()
    {
        NativeLanguageWord = "Word list completed!",
        ForeignLanguageWord = "Word list completed!"
    };

}
