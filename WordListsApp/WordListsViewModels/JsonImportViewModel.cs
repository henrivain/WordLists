using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Helpers;
using WordListsMauiHelpers.DeviceAccess;
using static WordDataAccessLibrary.CollectionBackupServices.BackupDelegates;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class JsonImportViewModel : IJsonImportViewModel
{
    // import checks, that path value is not this when starting import
    private const string DefaultPathInfo = "Valitse tiedosto napista";

    public JsonImportViewModel(
        ICollectionImportService importService, 
        IWordCollectionService dataBaseService)
    {
        ImportService = importService;
        CollectionDataBaseService = dataBaseService;
    }


    [ObservableProperty]
    string importPath = DefaultPathInfo;

    public IAsyncRelayCommand SelectFile => new AsyncRelayCommand(async () =>
    {
        string path = await FilePickerService.GetUserSelectedFullPath(new() 
        { 
            AppFileExtension.GetExtension(FileExtension.Wordlist) 
        });
        if (string.IsNullOrWhiteSpace(path) is false) ImportPath = path;
    });
    public IAsyncRelayCommand Import => new AsyncRelayCommand(
        async () => await ImportAndSaveCollectionsAsync());






    public event ImportFailEventHandler? EmptyImportAttempted;

    public event ImportFailEventHandler? ImportActionFailed;
    
    public event ImportSuccessfullEventHandler? ImportSuccessfull;

    private ICollectionImportService ImportService { get; }
    private IWordCollectionService CollectionDataBaseService { get; }
    
    private async Task ImportAndSaveCollectionsAsync()
    {
        ImportActionResult pathValidation = ValidatePath(ImportPath);
        if (pathValidation.Success is false)
        {
            EmptyImportAttempted?.Invoke(this, pathValidation);
            return;
        }

        ImportActionResult result;
        IEnumerable<IExportWordCollection> exportCollections;

        (result, exportCollections) = await ImportService.Import(pathValidation.UsedPath);
        if (result.Success is false)
        {
            ImportActionFailed?.Invoke(this, result);
            return;
        }

        await SaveConvertedCollections(exportCollections);
        ImportSuccessfull?.Invoke(this, new(BackupAction.ParseData)
        {
            MoreInfo = $"Imported and saved {exportCollections.Count()} word collections to database",
            Success = true
        });
    }

    private async Task SaveConvertedCollections(IEnumerable<IExportWordCollection> exportCollections)
    {
        List<WordCollection> wordCollections = exportCollections
            .Select(x => x.GetAsWordCollection())
            .Where(x => x is not null)
            .ToList();

        foreach (WordCollection wc in wordCollections)
        {
            await CollectionDataBaseService.AddWordCollection(wc);
        }
    }

    private static ImportActionResult ValidatePath(string path)
    {
        ImportActionResult result = new(BackupAction.Configure)
        {
            UsedPath = path,
            Success = true
        };

        if (path is not null && File.Exists(path) || path is DefaultPathInfo) return result;

        result.Success = false;
        result.MoreInfo = $"Given file path does not exist, see {nameof(ImportActionResult.UsedPath)}";
        return result;
    }
}
