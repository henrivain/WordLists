using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.Extensions;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class StartTrainingViewModel : IStartTrainingViewModel
{
    public StartTrainingViewModel(
        IWordCollectionOwnerService ownerService, 
        IWordCollectionService wordCollectionService,
        ILogger logger)
    {
        OwnerService = ownerService;
        WordCollectionService = wordCollectionService;
        Logger = logger;
        AllCollections = Enumerable.Empty<WordCollectionOwner>().ToList();
    }

    IWordCollectionOwnerService OwnerService { get; }
    IWordCollectionService WordCollectionService { get; }
    public ILogger Logger { get; }
    List<WordCollectionOwner> AllCollections { get; set; }

    [ObservableProperty]
    ObservableCollection<WordCollectionOwner> _visibleCollections = new();

    [ObservableProperty]
    string _searchTerm = string.Empty;

    public IRelayCommand FilterCollections => new RelayCommand(() =>
    {
        VisibleCollections.Clear();
        foreach (var collection in AllCollections)
        {
            if (Filter(collection))
            {
                VisibleCollections.Add(collection);
            }
        }

       
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && 
            VisibleCollections.Count < 4 && 
            VisibleCollections.Count > 0)
        {
            // These "placeholder" are hided in the ui because of their name
            // Some winui collectionview bug causes that items are only shown if there are at least 4
            // (grid items layout with span 3)
            WordCollectionOwner placeholder = new()
            {
                Name = "^^_$Placeholder$_^^"
            };
            for (int i = 0; i < 3; i++)
            {
                VisibleCollections.Add(placeholder);
            }
        }
    });


    public IAsyncRelayCommand UpdateCollections => new AsyncRelayCommand(async () =>
    {
        SearchTerm = string.Empty;
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
        Logger.LogInformation("Start training collection {id} with cards.", selectionId);
        var collection = await BuildSelectedWordCollection(selectionId);
        if (collection is null)
        {
            CollectionDoesNotExistEvent?.Invoke(this, selectionId, "Collection does not exit anymore. Try again.");
            return;
        }
        CardsTrainingRequestedEvent?.Invoke(this, new()
        {
            WordCollection = collection
        });
    });

    public IAsyncRelayCommand<int> RequestWriteTraining => new AsyncRelayCommand<int>(async selectionId =>
    {
        Logger.LogInformation("Start writing words of collection {id}.", selectionId);
        var collection = await BuildSelectedWordCollection(selectionId);
        if (collection is null)
        {
            CollectionDoesNotExistEvent?.Invoke(this, selectionId, "Collection does not exit anymore. Try again.");
            return;
        }
        WriteTrainingRequestedEvent?.Invoke(this, new()
        {
            WordCollection = collection
        });
    });

    public event TrainingRequestedEventHandler? CardsTrainingRequestedEvent;
    public event TrainingRequestedEventHandler? WriteTrainingRequestedEvent;
    public event DBKeyDoesNotExistEventHandler? CollectionDoesNotExistEvent;

    public async Task<WordCollection?> BuildSelectedWordCollection(int selectionId)
    {
        // returns null if collection id does not exist anymore

        WordCollection? collection = null;
        try
        {
            collection = await WordCollectionService.GetWordCollection(selectionId);
        }
        catch (InvalidOperationException)
        {
            // if view is not refreshed when collections are deleted from db
            Logger.LogInformation("{view}: Cannot start null wordcollection. Refreshing the view", nameof(StartTrainingViewModel));
            await ResetCollections();
            return null;
        }
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
        AllCollections = (await OwnerService.GetAll()).SortByName().ToList();
        FilterCollections.Execute(null);
        IsRefreshing = false;
    }

    private bool Filter(WordCollectionOwner? param)
    {
        if (param is null) return false;
        return param.LanguageHeaders.Contains(SearchTerm ?? string.Empty, StringComparison.OrdinalIgnoreCase) ||
            param.Name.Contains(SearchTerm ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }

}
