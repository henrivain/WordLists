namespace WordListsViewModels.Interfaces;
public interface IJsonExportViewModel
{
    List<WordCollection> AvailableWordCollections { get; }

    List<object> SelectedCollections { get; }

    IAsyncRelayCommand ExportSelections { get; }
    IAsyncRelayCommand ExportByName { get; }
    IAsyncRelayCommand ExportByLanguage { get; }
    IAsyncRelayCommand ExportByAll { get; }
}
