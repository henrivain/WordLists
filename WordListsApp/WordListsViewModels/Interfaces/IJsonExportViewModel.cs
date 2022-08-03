namespace WordListsViewModels.Interfaces;
public interface IJsonExportViewModel
{
    List<WordCollectionOwner> AvailableCollections { get; }

    List<object> SelectedCollections { get; }

    IAsyncRelayCommand ExportSelections { get; }
    IAsyncRelayCommand ExportByName { get; }
    IAsyncRelayCommand ExportByLanguage { get; }
    IAsyncRelayCommand ExportAllShown { get; }
}
