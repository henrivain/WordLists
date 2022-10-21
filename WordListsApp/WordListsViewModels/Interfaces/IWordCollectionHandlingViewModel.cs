using System.Collections.ObjectModel;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Interfaces;
public interface IWordCollectionHandlingViewModel
{

    ObservableCollection<WordCollectionInfo> AvailableCollections { get; set; }

    IAsyncRelayCommand UpdateCollectionInfos { get; }

    Task ResetCollections();

    IRelayCommand<int> RequestDelete { get; }

    IRelayCommand<int> Edit { get; }

    Task DeleteCollection(WordCollectionOwner owner);

    event DeleteWantedEventHandler? DeleteRequested;

    event CollectionDeletedEventHandler? CollectionDeleted;

    event EditWantedEventHandler? EditWanted;

}
