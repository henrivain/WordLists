using System.ComponentModel;
using WordListsViewModels.Events;

namespace WordListsViewModels.Interfaces;
public interface IStartTrainingViewModel
{
    List<WordCollectionOwner> AvailableCollections { get; set; }

    IAsyncRelayCommand FilterCollections { get; }

    IAsyncRelayCommand UpdateCollections { get; }

    IAsyncRelayCommand<int> RequestCardsTraining { get; }

    IAsyncRelayCommand<int> RequestWriteTraining { get; }

    string SearchTerm { get; set; }
    bool ShowLearnedWords { get; set; }
    bool ShowMightKnowWords { get; set; }
    bool ShowNeverHeardKnowWords { get; set; }
    bool IsRefreshing { get; set; }
    bool RandomizeWordPairsOrder { get; set; }

    Task ResetCollections();

    WordCollectionOwner SelectedItem { get; set; }

    event TrainingRequestedEventHandler CardsTrainingRequestedEvent;
    
    event TrainingRequestedEventHandler WriteTrainingRequestedEvent;

    event PropertyChangedEventHandler PropertyChanged;
}


