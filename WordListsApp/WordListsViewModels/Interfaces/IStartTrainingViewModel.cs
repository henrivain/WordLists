using System.Collections.ObjectModel;
using System.ComponentModel;
using WordListsViewModels.Events;

namespace WordListsViewModels.Interfaces;
public interface IStartTrainingViewModel
{
    ObservableCollection<WordCollectionOwner> VisibleCollections { get; set; }

    IRelayCommand FilterCollections { get; }

    IAsyncRelayCommand UpdateCollections { get; }

    IAsyncRelayCommand<int> RequestCardsTraining { get; }

    IAsyncRelayCommand<int> RequestWriteTraining { get; }

    string SearchTerm { get; set; }
    bool ShowLearnedWords { get; set; }
    bool ShowWeaklyKnownWords { get; set; }
    bool ShowUnheardWords { get; set; }
    bool IsRefreshing { get; set; }
    bool ShuffleWords { get; set; }

    Task ResetCollections();

    WordCollectionOwner SelectedItem { get; set; }

    event TrainingRequestedEventHandler CardsTrainingRequestedEvent;
    
    event TrainingRequestedEventHandler WriteTrainingRequestedEvent;

    event PropertyChangedEventHandler PropertyChanged;

    event DBKeyDoesNotExistEventHandler CollectionDoesNotExistEvent;
}


