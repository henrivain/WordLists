namespace WordListsViewModels.Interfaces;
public interface IJsonExportViewModel
{
    List<WordCollection> AvailableWordCollections { get; }

    List<WordCollection> SelectedCollections { get; }

    IAsyncRelayCommand ExportSelections { get; }
    IAsyncRelayCommand ExportByName { get; }
    IAsyncRelayCommand ExportByLanguage { get; }
    IAsyncRelayCommand ExportByAll { get; }
}
