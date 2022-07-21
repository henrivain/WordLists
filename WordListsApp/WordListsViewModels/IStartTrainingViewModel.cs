using CommunityToolkit.Mvvm.Input;
using WordDataAccessLibrary;
using static WordDataAccessLibrary.DataBaseActions.DataBaseDelegates;

namespace WordListsViewModels;
public interface IStartTrainingViewModel
{
    List<WordCollectionOwner> AvailableCollections { get; set; }

    IAsyncRelayCommand UpdateCollectionsByName { get; }

    IAsyncRelayCommand UpdateCollectionsByLanguage { get; }

    IAsyncRelayCommand UpdateCollections { get; }

    string DataParameter { get; set; }
    
    bool ShowLearnedWords { get; set; }
    bool ShowMightKnowWords { get; set; }
    bool ShowNeverHeardKnowWords { get; set; }
    bool IsRefreshing { get; set; }

    Task ResetCollections();

    WordCollectionOwner SelectedItem { get; set; }
}


