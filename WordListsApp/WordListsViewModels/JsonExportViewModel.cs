using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers;
using WordListsMauiHelpers.DeviceAccess;
using WordListsViewModels.Extensions;
using WordListsViewModels.Helpers;
using static WordDataAccessLibrary.CollectionBackupServices.BackupDelegates;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class JsonExportViewModel : IJsonExportViewModel
{
    public JsonExportViewModel(
        ICollectionExportService exportService, 
        IWordCollectionService collectionService,
        IWordCollectionInfoService collectionInfoService
        )
    {
        ExportService = exportService;
        CollectionService = collectionService;
        CollectionInfoService = collectionInfoService;
        _ = ResetCollections();
    }

    private ICollectionExportService ExportService { get; }
    public IWordCollectionService CollectionService { get; }
    public IWordCollectionInfoService CollectionInfoService { get; }

    [ObservableProperty]
    bool removeUserDataFromWordPairs = true;

    [ObservableProperty]
    bool canExportAllVisible = false;

    [ObservableProperty]
    bool canExportSelected = false;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(VisibleCollections))]
    string nameParameter = string.Empty;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(VisibleCollections))]
    string languageHeadersParameter = string.Empty;

    public List<WordCollectionInfo> VisibleCollections 
    {
        get 
        {
            List<WordCollectionInfo> fittingCollections = AvailableCollections
                                       .Where(x => x.Owner.LanguageHeaders.Contains(LanguageHeadersParameter))
                                       .Where(x => x.Owner.Name.Contains(NameParameter))
                                       .ToList();

            CanExportAllVisible = fittingCollections.Count > 0;
            return fittingCollections;
        } 
    }

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(VisibleCollections))]
    List<WordCollectionInfo> availableCollections = new(){};


    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(CanExportSelected))]
    List<object> selectedCollections = new();



    [ObservableProperty]
    string exportPath = PathHelper.GetDefaultBackupFilePath();

    public IAsyncRelayCommand ExportAllVisibleCommand => new AsyncRelayCommand(async () =>
    {
        await Export(VisibleCollections);
    });
    public IAsyncRelayCommand ExportSelectionsCommand => new AsyncRelayCommand(async () =>
    {
        await Export(SelectedCollections.GetOwners());
    });
    public IAsyncRelayCommand ChooseExportLocationCommand => new AsyncRelayCommand(async () =>
    {
        string exportPath = await FilePickerService.GetUserSelectedExportPath();
        if (string.IsNullOrWhiteSpace(exportPath)) return;
        ExportPath = exportPath;
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
        AvailableCollections = await CollectionInfoService.GetAll();
    }



    public event ExportFailEventHandler? EmptyExportAttempted;
    public event ExportSuccessfullEventHandler? ExportCompleted;







    private async Task Export(List<WordCollectionInfo> infos)
    {
        string path = ExportPath;

        List<WordCollectionOwner> owners = infos.Select(x => x.Owner).ToList();

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

        ExportActionResult result = await ExportService.ExportByCollectionOwners(owners, path, RemoveUserDataFromWordPairs);
        ExportCompleted?.Invoke(this, result);
    }

    private void InvokeEmptyExportEvent(string text)
    {
        EmptyExportAttempted?.Invoke(this, new(BackupAction.Configure)
        {
            Success = false,
            MoreInfo = text
        });
    }


}
