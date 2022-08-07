using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.ActionResults;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Helpers;
using WordListsMauiHelpers.DeviceAccess;
using static WordDataAccessLibrary.CollectionBackupServices.BackupDelegates;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class JsonImportViewModel : IJsonImportViewModel
{
    // import checks, that path value is not this when starting import
    private const string DefaultPathInfo = "Valitse tiedosto...";

    readonly string[] _acceptableFileExtensions = GetAcceptableImportFileExtensions();

    public JsonImportViewModel(
        ICollectionImportService importService, 
        IWordCollectionService dataBaseService)
    {
        ImportService = importService;
        CollectionDataBaseService = dataBaseService;
    }


    [ObservableProperty]
    string importPath = DefaultPathInfo;

    [ObservableProperty]
    bool canImport = false;
    public string AcceptableFileExtensions { get; } = 
        string.Join(" ", GetAcceptableImportFileExtensions());

    public IAsyncRelayCommand SelectFile => new AsyncRelayCommand(async () =>
    {
        string path = await FilePickerService.GetUserSelectedFullPath(new(_acceptableFileExtensions));
        if (string.IsNullOrWhiteSpace(path)) return;

        SetPathDataIfValid(path);
    });

    private void SetPathDataIfValid(string path)
    {
        FileActionResult result = new(FileAction.ValidateType)
        {
            Success = false,
            UsedPath = path,
        };
        if (PathHasValidExtension(path))
        {
            result.Success = true;
            SelectFileAttempted?.Invoke(this, result);
            ImportPath = path;
            CanImport = true;
            return;
        }
        result.MoreInfo = $"Filetype {Path.GetExtension(path)} is not valid file" +
                   $" type for import, type must be one of {AcceptableFileExtensions}";
        SelectFileAttempted?.Invoke(this, result);
    }


    public IAsyncRelayCommand Import => new AsyncRelayCommand(
        async () => await ImportAndSaveCollectionsAsync());


    public event ImportFailEventHandler? EmptyImportAttempted;

    public event ImportFailEventHandler? ImportActionFailed;
    
    public event ImportSuccessfullEventHandler? ImportSuccessfull;
    
    public event FileAccessEventHandler? SelectFileAttempted;

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
            MoreInfo = $"Tuotu ja tallennettu {exportCollections.Count()} sanastoa",
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
    private static string[] GetAcceptableImportFileExtensions()
    {
        string[] extensions = { AppFileExtension.Get(FileExtension.Wordlist) };
        return extensions;
    }
    private bool PathHasValidExtension(string path)
    {
        return _acceptableFileExtensions.Any(x => Path.GetExtension(path) == x);
    }

}
