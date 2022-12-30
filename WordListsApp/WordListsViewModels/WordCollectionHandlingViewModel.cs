using System.Collections.ObjectModel;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordCollectionHandlingViewModel : IWordCollectionHandlingViewModel
{
    public WordCollectionHandlingViewModel(IWordCollectionService collectionService, ILogger<IWordCollectionHandlingViewModel> logger)
    {
        CollectionService = collectionService;
        Logger = logger;
    }

    [ObservableProperty]
    ObservableCollection<WordCollectionInfo> _availableCollections = new();

    public event CollectionDeletedEventHandler? CollectionsDeleted;

    public event DeleteWantedEventHandler? DeleteRequested;

#pragma warning disable CS0067 // EditRequested is never used
    public event EditWantedEventHandler? EditRequested;
#pragma warning restore CS0067 // EditRequested is never used 

    public IAsyncRelayCommand UpdateCollectionInfos => new AsyncRelayCommand(ResetCollections);

    public IWordCollectionService CollectionService { get; }
    ILogger<IWordCollectionHandlingViewModel> Logger { get; }

    


    public IRelayCommand<int> Edit => new RelayCommand<int>((value) =>
    {
        throw new NotImplementedException();
    });

    public IRelayCommand<int> VerifyDeleteCommand => new RelayCommand<int>((value) =>
    {
        var ownerInfo = AvailableCollections.FirstOrDefault(x => x.Owner.Id == value);

        DeleteRequested?.Invoke(this, new DeleteWantedEventArgs
        {
            ItemsToDelete = new[] { ownerInfo.Owner },
            DeletesAll = false
        });
    });
    public IRelayCommand VerifyDeleteAllCommand => new RelayCommand(() =>
    {
        DeleteWantedEventArgs args = new()
        {
             ItemsToDelete = AvailableCollections.Select(x => x.Owner).ToArray(),
             DeletesAll = true
        };
        DeleteRequested?.Invoke(this, args);
    });

    public async Task ResetCollections()
    {
        List<WordCollection> collections = await CollectionService.GetWordCollections();

        List<WordCollectionInfo> collectionInfos = new();
        foreach (var collection in collections)
        {
            collectionInfos.Add(new(collection.Owner, collection.WordPairs.Count));
        }
        AvailableCollections = new(collectionInfos.OrderBy(x => x.Owner.Name));
    }
    public async Task DeleteCollections(WordCollectionOwner[] owners)
    {
        Logger.LogInformation("User requested to delete '{amount}' word collections.", owners.Length);
        if (owners is null)
        {
            Logger.LogError("Attempt to remove collections in '{modelName}' failed because '{owners}' given value was null",
                nameof(WordCollectionHandlingViewModel), nameof(owners));
            return;
        }
        foreach (var owner in owners)
        {
            var matching = AvailableCollections.FirstOrDefault(x => x.Owner.Id == owner.Id);
            AvailableCollections.Remove(matching);
            await CollectionService.DeleteWordCollection(owner.Id);
        }
        Logger.LogInformation("Deleted '{amount}' word collections from database.", owners.Length);

        CollectionsDeleted?.Invoke(this, new()
        {
            Text = $"Deleted {owners.Length} {nameof(WordCollection)}s from data base successfully",
            CollectionNames = owners.Select(x => x.Name).ToArray(),
            RefIds = owners.Select(x => x.Id).ToArray()
        });
    }
}
