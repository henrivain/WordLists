using System.Collections.ObjectModel;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.Extensions;
using WordListsViewModels.Events;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class StartTrainingViewModel : IStartTrainingViewModel
{
    public StartTrainingViewModel(IWordCollectionOwnerService ownerService, IWordCollectionService wordCollectionService)
    {
        OwnerService = ownerService;
        WordCollectionService = wordCollectionService;
        AllCollections = Enumerable.Empty<WordCollectionOwner>().ToList();
    }

    IWordCollectionOwnerService OwnerService { get; }
    IWordCollectionService WordCollectionService { get; }

    List<WordCollectionOwner> AllCollections { get; set; }

    [ObservableProperty]
    List<WordCollectionOwner> _availableCollections = new();

    [ObservableProperty]
    string _searchTerm = string.Empty;

    int _filtersWaiting = 0;

    public IAsyncRelayCommand FilterCollections => new AsyncRelayCommand(async () =>
    {
        _filtersWaiting++;
        await Task.Delay(250);
        if (_filtersWaiting > 1)
        {
            _filtersWaiting--;
            return;
        }
        _filtersWaiting = 0;

        AvailableCollections = await Task.Run(() =>
        {
            return AllCollections.Where(Filter).ToList();
        });


        //foreach (var collection in AllCollections)
        //{
        //    if (Filter(collection))
        //    {
        //        if (AvailableCollections.Contains(collection))
        //        {
        //            continue;
        //        }
        //        AvailableCollections.Add(collection);
        //        continue;
        //    }
        //    AvailableCollections.Remove(collection);
        //}



        //var filtered = AllCollections.Where(Filter);
        //foreach (var collection in AllCollections)
        //{
        //    if (filtered.Contains(collection) is false)
        //    {
        //        Debug.WriteLine($"Try remove {collection.Name}");
        //        AvailableCollections.Remove(collection);
        //        Debug.WriteLine($"Removed {collection.Name}");
        //        continue;
        //    }
        //    if (AvailableCollections.Contains(collection) is false)
        //    {
        //        Debug.WriteLine($"Try add {collection.Name}");
        //        AvailableCollections.Add(collection);
        //        Debug.WriteLine($"Added {collection.Name}");
        //        continue;
        //    }
        //}
    });


    public IAsyncRelayCommand UpdateCollections => new AsyncRelayCommand(ResetCollections);





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
        AllCollections = (await OwnerService.GetAll()).SortByName().ToList();
        AvailableCollections = new(AllCollections);
        IsRefreshing = false;
    }

    private bool Filter(WordCollectionOwner? param)
    {
        if (param is null) return false;
        return param.LanguageHeaders.Contains(SearchTerm ?? string.Empty, StringComparison.OrdinalIgnoreCase) ||
            param.Name.Contains(SearchTerm ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }

}
