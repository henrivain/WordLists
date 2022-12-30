using System.Collections.ObjectModel;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Interfaces;
public interface IWordCollectionHandlingViewModel
{

    ObservableCollection<WordCollectionInfo> AvailableCollections { get; set; }

    IAsyncRelayCommand UpdateCollectionInfos { get; }

    Task ResetCollections();

    IRelayCommand<int> VerifyDeleteCommand { get; }

    IRelayCommand VerifyDeleteAllCommand { get; }

    IRelayCommand<int> Edit { get; }

    Task DeleteCollections(WordCollectionOwner[] owners);

    event DeleteWantedEventHandler? DeleteRequested;

    event CollectionDeletedEventHandler? CollectionsDeleted;

    event EditWantedEventHandler? EditRequested;

}
