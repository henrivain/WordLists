using CommunityToolkit.Mvvm.Input;
using WordDataAccessLibrary;

namespace WordListsViewModels;
public interface IStartTrainingViewModel
{
    List<WordCollectionOwner> AvailableCollections { get; }

    IAsyncRelayCommand UpdateCollectionsByName { get; }

    IAsyncRelayCommand UpdateCollectionsByLanguage { get; }

    string DataParameter { get; set; }

    Task ResetCollections();
}
