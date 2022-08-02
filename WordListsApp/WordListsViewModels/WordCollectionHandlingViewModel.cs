using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordListsViewModels.Helpers;
using static WordDataAccessLibrary.DataBaseActions.DataBaseDelegates;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordCollectionHandlingViewModel : IWordCollectionHandlingViewModel
{
    [ObservableProperty]
    List<WordCollectionInfo> availableCollections = new();


    public event CollectionDeletedEventHandler? CollectionDeleted;

    public IAsyncRelayCommand UpdateCollectionInfos => new AsyncRelayCommand(async () =>
    {
        await ResetCollections();
    });


    public async Task ResetCollections()
    {
        List<WordCollection> collections = await WordCollectionService.GetWordCollections();

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
        
        await WordCollectionService.DeleteWordCollection(id);

        await ResetCollections();

        CollectionDeleted?.Invoke(this, new(
            id,
            text: $"Deleted {nameof(WordCollection)} from database with given id",
            collectionName: info.Owner.Name
            ));
        
    }
}
