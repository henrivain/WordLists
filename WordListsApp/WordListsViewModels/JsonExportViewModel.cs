using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers;
using WordListsMauiHelpers.DeviceAccess;
using WordListsViewModels.Helpers;
using static WordDataAccessLibrary.CollectionBackupServices.BackupDelegates;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class JsonExportViewModel : IJsonExportViewModel
{
    public JsonExportViewModel(
        ICollectionExportService exportService,
        IWordCollectionInfoService collectionInfoService,
        IFilePickerService filePickerService,
        ILogger logger
        )
    {
        string folder = PathHelper.GetDownloadsFolderPath();
        string extension = filePickerService.GetAppFileExtension();
        _exportFileExtension = extension;
        _exportFolderPath = folder;
        _exportFileName = GetNewExportFileName(folder, "wordlist_export_", extension);

        ExportService = exportService;
        CollectionInfoService = collectionInfoService;
        FilePickerService = filePickerService;
        Logger = logger;
        _ = ResetCollections();
    }

 

    ICollectionExportService ExportService { get; }
    IWordCollectionInfoService CollectionInfoService { get; }
    IFilePickerService FilePickerService { get; }
    public ILogger Logger { get; }
    List<WordCollectionInfo> AvailableCollections { get; set; } = Enumerable.Empty<WordCollectionInfo>().ToList();

    [ObservableProperty]
    ObservableCollection<WordCollectionInfo> _visibleCollections = new();

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanExportSelected))]
    List<object> _selectedCollections = new();

    [ObservableProperty]
    bool _removeUserDataFromWordPairs = true;

    [ObservableProperty]
    bool _canExportAllVisible = false;

    [ObservableProperty]
    bool _canExportSelected = false;

    [ObservableProperty]
    string _nameFilter = string.Empty;

    [ObservableProperty]
    string _languageFilter = string.Empty;


    [ObservableProperty]
    string _exportFileName;

    [ObservableProperty]
    string _exportFolderPath;

    [ObservableProperty]
    string _exportFileExtension;

    public string ExportPath => Path.Combine(ExportFolderPath, $"{ExportFileName}{ExportFileExtension}");



    public IRelayCommand SearchParameterChangedCommand => new RelayCommand(() =>
    {
        VisibleCollections.Clear();
        foreach (var collection in AvailableCollections)
        {
            if (Filter(collection))
            {
                VisibleCollections.Add(collection);
            }
        }
        CanExportAllVisible = VisibleCollections.Count > 0;
    });
    public IAsyncRelayCommand ExportAllVisibleCommand => new AsyncRelayCommand(async () =>
    {
        await Export(VisibleCollections.ToList());
    });
    public IAsyncRelayCommand ExportSelectionsCommand => new AsyncRelayCommand(async () =>
    {
        await Export(SelectedCollections.GetOwners());
    });
    public IAsyncRelayCommand ChooseExportFolderCommand => new AsyncRelayCommand(async () =>
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) is false)
        {
            Logger.LogWarning("Cannot choose export folder, because platform is not Windows.");
            return;
        }
        string? exportFolder = await FilePickerService.PickFolder();
        if (string.IsNullOrWhiteSpace(exportFolder)) return;
        Logger.LogInformation("User changed export folder to be '{path}'.", exportFolder);
        ExportFolderPath = exportFolder;
    });
    public IAsyncRelayCommand CopyPathToClipBoardCommand => new AsyncRelayCommand(async () =>
    {
        await ClipboardAccess.SetStringAsync(ExportPath);
    });
    public IRelayCommand SelectionChangedCommand => new RelayCommand(() =>
    {
        CanExportSelected = SelectedCollections.Count > 0;
    });

    public async Task ResetCollections()
    {
        Logger.LogInformation("Reset {name} Wordcollections.", nameof(JsonExportViewModel));
        AvailableCollections = await CollectionInfoService.GetAll();
        VisibleCollections.Clear();
        SearchParameterChangedCommand.Execute(null);
    }


    public event ExportFailEventHandler? EmptyExportAttempted;
    public event ExportSuccessfullEventHandler? ExportCompleted;

    private async Task Export(List<WordCollectionInfo> infos)
    {
        Logger.LogInformation("Export {count} wordcollections.", infos.Count);

        List<WordCollectionOwner> owners = infos.Select(x => x.Owner).ToList();
        string path = ExportPath;

        if (owners is null || owners.Count == 0)
        {
            InvokeEmptyExportEvent((owners is null) ? "owners is null" : "owners is empty list");
            return;
        }
        if (string.IsNullOrWhiteSpace(path))
        {
            InvokeEmptyExportEvent($"{nameof(path)} is null or empty");
            return;
        }

        Logger.LogInformation("Use export path '{path}'.", path);
        ExportActionResult result = await ExportService.ExportByCollectionOwners(owners, path, RemoveUserDataFromWordPairs);

        Logger.LogInformation("Export finished. Success: '{success}', Msg: '{msg}'.", result.Success, result.MoreInfo);
        ExportCompleted?.Invoke(this, result);
    }
    private void InvokeEmptyExportEvent(string text)
    {
        Logger.LogInformation("User tried to export wordlists, when none was selected: '{text}'", text);
        EmptyExportAttempted?.Invoke(this, new(BackupAction.Configure)
        {
            Success = false,
            MoreInfo = text
        });
    }
    private bool Filter(WordCollectionInfo info)
    {
        bool isValidName = info.Owner.Name.Contains(NameFilter ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        bool isValidLang = info.Owner.LanguageHeaders.Contains(LanguageFilter ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        return isValidName && isValidLang;
    }
    private static string GetTimeString()
    {
        return DateTime.Now.ToString("G")
                .Replace(" ", string.Empty)
                .Replace("/", string.Empty)
                .Replace(":", string.Empty)
                .Replace("PM", string.Empty)
                .Replace("AM", string.Empty)
                .Replace(".", string.Empty);
    }
    private static string GetNewExportFileName(string folderPath, string nameStart, string extension)
    {
        string newPath;
        int num = 0;
        do
        {
            num++;
            string fileName = $"{nameStart}{num}{extension}";
            newPath = Path.Combine(folderPath, fileName);
            if (num > 200)
            {
                break;
            }
        }
        while (Path.Exists(newPath));
        return $"{nameStart}{num}";
    }

}
