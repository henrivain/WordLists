using System.ComponentModel.DataAnnotations;
using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.ActionResults;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Extensions;
using WordDataAccessLibrary.Helpers;
using WordListsMauiHelpers.DeviceAccess;
using static WordDataAccessLibrary.CollectionBackupServices.BackupDelegates;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class JsonImportViewModel : IJsonImportViewModel
{
    // import checks, that path value is not this when starting import
    const string DefaultPathInfo = "Valitse tiedosto...";
    readonly string[] _validExtensions = GetValidExtensions();



    public JsonImportViewModel(
        ICollectionImportService importService,
        IWordCollectionService dataBaseService,
        ILogger<IJsonImportViewModel> logger)
    {
        ImportService = importService;
        CollectionDataBaseService = dataBaseService;
        Logger = logger;
    }


    [ObservableProperty]
    string _importPath = DefaultPathInfo;

    [ObservableProperty]
    bool _canImport = false;

    [ObservableProperty]
    bool _isBusy = false;

    public string AcceptableFileExtensions { get; } =
        string.Join(", ", GetValidExtensions());

    public IAsyncRelayCommand SelectFile => new AsyncRelayCommand(async () =>
    {
        string? path = await DeviceSpecificFilePicker.GetUserSelectedFullPath(new(_validExtensions));
        if (string.IsNullOrWhiteSpace(path)) return;

        SetPathIfValid(path);
    });

    public IAsyncRelayCommand Import => new AsyncRelayCommand(
        async () => await ImportAndSaveCollectionsAsync());

    public void SetDefaultImportPath(string path)
    {
        if (path is not null && Path.Exists(path))
        {
            Logger.LogInformation("Successfully set import path to '{path}'", path);
            ImportPath = path;
            CanImport = true;
            return;
        }
        Logger.LogWarning("Cannot set {prop} to have value '{path}', " +
            "because path is invalid or doens't exist.", nameof(ImportPath), path);
    }




    ICollectionImportService ImportService { get; }
    IWordCollectionService CollectionDataBaseService { get; }
    ILogger<IJsonImportViewModel> Logger { get; }


    public event ImportFailEventHandler? EmptyImportAttempted;

    public event ImportFailEventHandler? ImportActionFailed;

    public event ImportSuccessfullEventHandler? ImportSuccessfull;

    public event FileAccessEventHandler? SelectFileAttempted;

    private void SetPathIfValid(string path)
    {
        bool isValid = HasValidExtension(path);
        if (isValid)
        {
            ImportPath = path;
            CanImport = true;
        }
        SelectFileAttempted?.Invoke(this, new FileActionResult
        {
            FileAction = FileAction.ValidateType,
            Success = isValid,
            UsedPath = path,
            MoreInfo = isValid ? string.Empty : 
                $"Invalid file extension '{Path.GetExtension(path)}', " +
                $"only extensions '{AcceptableFileExtensions}' are valid."
        });
    }
    private async Task ImportAndSaveCollectionsAsync()
    {
        ImportActionResult validateResult = ValidatePath(ImportPath);
        if (validateResult.NotSuccess())
        {
            EmptyImportAttempted?.Invoke(this, validateResult);
            return;
        }
        IsBusy = true;

        var (importResult, collections) = await ImportService.Import(validateResult.UsedPath);
        if (importResult.NotSuccess())
        {
            IsBusy = false;
            ImportActionFailed?.Invoke(this, importResult);
            return;
        }

        List<WordCollection> converted = collections
           .Select(x => x.GetAsWordCollection())
           .Where(x => x is not null)
           .ToList();

        foreach (var wc in converted)
        {
            await CollectionDataBaseService.AddWordCollection(wc);
        }
        Logger.LogInformation("Imported '{count}' collections from file.", converted.Count);
        ImportSuccessfull?.Invoke(this, new ImportActionResult()
        {
            ActionType = BackupAction.ParseData,
            MoreInfo = $"Tuotu ja tallennettu {converted.Count} sanastoa",
            Success = true
        });
        IsBusy = false;
    }

    private static ImportActionResult ValidatePath(string path)
    {
        bool isValid = path is not null && File.Exists(path);
        return new ImportActionResult
        {
            ActionType = BackupAction.Configure,
            Success = isValid, 
            UsedPath = path,
            MoreInfo = isValid ? string.Empty : $"Cannot import from invalid path '{path}'."
        };
    }
    private bool HasValidExtension(string path)
    {
        return _validExtensions.Any(x => Path.GetExtension(path) == x);
    }
    private static string[] GetValidExtensions()
    {
        return new string[] { AppFileExtension.Get(FileExtension.Wordlist) };
    }
}
