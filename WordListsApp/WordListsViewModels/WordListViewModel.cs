using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordListsViewModels;

public partial class WordListViewModel : ObservableObject, IWordListViewModel
{
    public WordListViewModel(IWordCollectionService collectionService, IWordCollectionOwnerService ownerService)
    {
        CollectionService = collectionService;
        OwnerService = ownerService;
        _ = SetPlaceHolderCollection();
    }

    [ObservableProperty]
    WordCollection _collection = new();

    IWordCollectionService CollectionService { get; }
    IWordCollectionOwnerService OwnerService { get; }

    async Task SetPlaceHolderCollection()
    {
        List<WordCollectionOwner> owners = await OwnerService.GetAll();
        int? id = owners.FirstOrDefault()?.Id;
        if (id is null)
        {
            throw new NullReferenceException(nameof(id));
        }
        Collection = await CollectionService.GetWordCollection(id.Value);
    }
}
