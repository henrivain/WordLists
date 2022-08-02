using CommunityToolkit.Mvvm.Input;
using WordDataAccessLibrary;
using WordListsViewModels.Helpers;
using static WordDataAccessLibrary.DataBaseActions.DataBaseDelegates;

namespace WordListsViewModels.Interfaces;
public interface IWordCollectionHandlingViewModel
{

    List<WordCollectionInfo> AvailableCollections { get; set; }

    IAsyncRelayCommand UpdateCollectionInfos { get; }

    Task ResetCollections();

    Task DeleteCollection(int id);

    event CollectionDeletedEventHandler? CollectionDeleted;
}
