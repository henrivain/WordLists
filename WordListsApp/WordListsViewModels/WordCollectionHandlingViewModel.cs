using CommunityToolkit.Mvvm.ComponentModel;
using WordDataAccessLibrary;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordCollectionHandlingViewModel : IWordCollectionHandlingViewModel
{
    [ObservableProperty]
    WordCollectionOwner? selected;

    [ObservableProperty]
    List<WordCollectionOwner> availableCollections = new();
}
