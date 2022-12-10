using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.Extensions;
using WordListsViewModels.Events;
using WordListsViewModels.Extensions;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class StartTrainingViewModel : IStartTrainingViewModel
{
    public StartTrainingViewModel(IWordCollectionOwnerService ownerService, IWordCollectionService wordCollectionService)
    {
        OwnerService = ownerService;
        WordCollectionService = wordCollectionService;
    }

    IWordCollectionOwnerService OwnerService { get; }
    IWordCollectionService WordCollectionService { get; }

    [ObservableProperty]
    List<WordCollectionOwner> _availableCollections = new();

    [ObservableProperty]
    string _dataParameter = string.Empty;
    
    
    public IAsyncRelayCommand UpdateCollectionsByName => new AsyncRelayCommand(async () =>
    {
        AvailableCollections = (await OwnerService.GetByName(DataParameter)).SortByName().ToList();
    });    
    
    public IAsyncRelayCommand UpdateCollectionsByLanguage => new AsyncRelayCommand(async () =>
    {
        AvailableCollections = (await OwnerService.GetByLanguage(DataParameter)).SortByName().ToList();
    });

    public IAsyncRelayCommand UpdateCollections => new AsyncRelayCommand(async () =>
    {
        await ResetCollections();
    });

    

    [ObservableProperty]
    bool _showLearnedWords = true;

    [ObservableProperty]
    bool _showMightKnowWords = true;

    [ObservableProperty]
    bool _showNeverHeardKnowWords = true;

    [ObservableProperty]
    int _showWords = 1;

    [ObservableProperty]
    bool _removeLearnedWords;

    [ObservableProperty]
    bool _removeMightKnowWords;

    [ObservableProperty]
    bool _removeNeverHeardWords;

    [ObservableProperty]
    bool _isRefreshing = false;

    [ObservableProperty]
    bool _randomizeWordPairsOrder = false;

    [ObservableProperty]
    WordCollectionOwner _selectedItem = new();


  

    public IAsyncRelayCommand<int> RequestCardsTraining => new AsyncRelayCommand<int>(async selectionId =>
    {
        CardsTrainingRequestedEvent?.Invoke(this, new()
        {
            WordCollection = await BuildSelectedWordCollection(selectionId)
        });
    });

    public IAsyncRelayCommand<int> RequestWriteTraining => new AsyncRelayCommand<int>(async selectionId =>
    {
        WriteTrainingRequestedEvent?.Invoke(this, new()
        {
            WordCollection = await BuildSelectedWordCollection(selectionId)
        });
    });

    public event TrainingRequestedEventHandler? CardsTrainingRequestedEvent;
    public event TrainingRequestedEventHandler? WriteTrainingRequestedEvent;

    public async Task<WordCollection> BuildSelectedWordCollection(int selectionId)
    {
        WordCollection collection = await WordCollectionService.GetWordCollection(selectionId);

        if (ShowLearnedWords is false)
        {
            collection.WordPairs = collection.WordPairs
                .Where(x => x.LearnState is not WordLearnState.Learned)
                    .ToList();
        }
        if (ShowMightKnowWords is false)
        {
            collection.WordPairs = collection.WordPairs
                .Where(x => x.LearnState is not WordLearnState.MightKnow)
                    .ToList();
        }
        if (ShowNeverHeardKnowWords is false)
        {
            collection.WordPairs = collection.WordPairs
                .Where(x => x.LearnState is not WordLearnState.NeverHeard)
                    .ToList();
        }
        if (RandomizeWordPairsOrder)
        {
            collection.WordPairs = collection.WordPairs.Shuffle();
        }
        return collection;
    }

    public async Task ResetCollections()
    {
        IsRefreshing = true;   
        AvailableCollections = (await OwnerService.GetAll()).SortByName().ToList();
        IsRefreshing = false;
    }
}
