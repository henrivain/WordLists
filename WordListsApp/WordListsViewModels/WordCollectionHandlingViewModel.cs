using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsViewModels.Events;
using WordListsViewModels.Extensions;
using WordListsViewModels.Helpers;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordCollectionHandlingViewModel : IWordCollectionHandlingViewModel
{
    public WordCollectionHandlingViewModel(IWordCollectionService collectionService)
    {
        CollectionService = collectionService;
    }

    [ObservableProperty]
    ObservableCollection<WordCollectionInfo> availableCollections = new();

    public event CollectionDeletedEventHandler? CollectionDeleted;

    public event DeleteWantedEventHandler? DeleteRequested;

#pragma warning disable CS0067
    public event EditWantedEventHandler? EditWanted;
#pragma warning restore CS0067

    public IAsyncRelayCommand UpdateCollectionInfos => new AsyncRelayCommand(async () =>
    {
        await ResetCollections();
    });

    public IWordCollectionService CollectionService { get; }

    public IRelayCommand<int> RequestDelete => new RelayCommand<int>((value) =>
    {
        var ownerInfo = AvailableCollections.FirstOrDefault(x => x.Owner.Id == value);
        DeleteRequested?.Invoke(this, new(ownerInfo.Owner));
    });

    public IRelayCommand<int> Edit => new RelayCommand<int>((value) =>
    {
        throw new NotImplementedException();
    });


    public async Task ResetCollections()
    {
        List<WordCollection> collections = await CollectionService.GetWordCollections();

        ObservableCollection<WordCollectionInfo> collectionInfos = new();
        foreach (var collection in collections)
        {
            collectionInfos.Add(new(collection.Owner, collection.WordPairs.Count));
        }
        AvailableCollections = new(collectionInfos.OrderBy(x => x.Owner.Name));
    }

    public async Task DeleteCollection(WordCollectionOwner owner)
    {
        try
        {
            var selected = AvailableCollections.First(x => x.Owner.Id == owner.Id);
            bool success = AvailableCollections.Remove(selected);
            await CollectionService.DeleteWordCollection(owner.Id);
            if (success)
            {
                OnPropertyChanged(nameof(AvailableCollections));
                CollectionDeleted?.Invoke(this, new(
                   owner.Id,
                   text: $"Deleted {nameof(WordCollection)} '{owner.Name}' from database with id '{owner.Id}'",
                   collectionName: owner?.Name ?? "NULL"
                   ));
            }
        }
        catch (ArgumentNullException)
        {
            Debug.WriteLine($"Attempt in {nameof(WordCollectionHandlingViewModel)} to remove collection with owner id failed because given value was null");
        }
    }
}
