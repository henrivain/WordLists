using WordListsViewModels.Helpers;

namespace WordListsViewModels.Interfaces;
public interface IWordCollectionHandlingViewModel
{

    List<WordCollectionInfo> AvailableCollections { get; set; }

    IAsyncRelayCommand UpdateCollectionInfos { get; }

    Task ResetCollections();

    Task DeleteCollection(int id);

    event CollectionDeletedEventHandler? CollectionDeleted;
}
