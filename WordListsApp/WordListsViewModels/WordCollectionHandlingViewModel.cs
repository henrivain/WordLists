using WordListsViewModels.Extensions;
using WordListsViewModels.Helpers;
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordCollectionHandlingViewModel : IWordCollectionHandlingViewModel
{
    public WordCollectionHandlingViewModel(IWordCollectionService collectionService)
    {
        CollectionService = collectionService;
    }

    [ObservableProperty]
    List<WordCollectionInfo> availableCollections = new();


    public event CollectionDeletedEventHandler? CollectionDeleted;

    public IAsyncRelayCommand UpdateCollectionInfos => new AsyncRelayCommand(async () =>
    {
        await ResetCollections();
    });

    public IWordCollectionService CollectionService { get; }

    public async Task ResetCollections()
    {
        List<WordCollection> collections = await CollectionService.GetWordCollections();

        List<WordCollectionInfo> collectionInfos = new();
        foreach (var collection in collections)
        {
            collectionInfos.Add(new(collection.Owner, collection.WordPairs.Count));
        }
        AvailableCollections = collectionInfos.SortByName();
    }
    
    public async Task DeleteCollection(int id)
    {
        WordCollectionInfo info = AvailableCollections.FirstOrDefault(x => x.Owner.Id == id);
        int deletedobjects = await CollectionService.DeleteWordCollection(id);

        await ResetCollections();

        CollectionDeleted?.Invoke(this, new(
            id,
            text: $"Deleted {nameof(WordCollection)} from database with given id",
            collectionName: info.Owner?.Name ?? "NULL"
            ));
        
    }
}
