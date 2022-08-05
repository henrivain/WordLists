using WordListsMauiHelpers;
using WordListsMauiHelpers.DeviceAccess;
using WordListsViewModels.Extensions;
using WordListsViewModels.Helpers;
using WordDataAccessLibrary.DataBaseActions.Interfaces;

using static WordDataAccessLibrary.ExportServices.ExportDelegates;
using WordDataAccessLibrary.ExportServices;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class JsonExportViewModel : IJsonExportViewModel
{
    public JsonExportViewModel(
        IExportService exportService, 
        IWordCollectionService collectionService,
        IWordCollectionInfoService collectionInfoService
        )
    {
        ExportService = exportService;
        CollectionService = collectionService;
        CollectionInfoService = collectionInfoService;
        _ = ResetCollections();
    }

    private IExportService ExportService { get; }
    public IWordCollectionService CollectionService { get; }
    public IWordCollectionInfoService CollectionInfoService { get; }

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
            return AvailableCollections.Where(x => x.Owner.LanguageHeaders.Contains(LanguageHeadersParameter))
                                       .Where(x => x.Owner.Name.Contains(NameParameter))
                                       .ToList(); 
        } 
    }

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(VisibleCollections))]
    List<WordCollectionInfo> availableCollections = new(){};

    [ObservableProperty]
    List<object> selectedCollections = new();

    [ObservableProperty]
    string exportPath = PathHelper.GetDefaultExportFilePath();


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
        string exportPath = await FilePickerService.GetUserSelectedJsonExportPath();
        if (string.IsNullOrWhiteSpace(exportPath)) return;
        ExportPath = exportPath;
    });
    public IAsyncRelayCommand CopyPathToClipBoardCommand => new AsyncRelayCommand(async () =>
    {
        await ClipboardAccess.SetStringAsync(ExportPath);
    });

    public async Task ResetCollections()
    {
        AvailableCollections = await CollectionInfoService.GetAll();
    }
    private async Task Export(List<WordCollectionInfo> infos)
    {
        string path = ExportPath;

        List<WordCollectionOwner> owners = infos.Select(x => x.Owner).ToList();

        if (owners is null || owners.Count == 0)
        {
            EmptyExportAttempted?.Invoke(this, new(ExportAction.ConfigureExport)
            {
                Success = false,
                MoreInfo = (owners is null) ? "owners is null" : "owners is empty list"
            });
            return;
        }
        if (string.IsNullOrWhiteSpace(path))
        {
            EmptyExportAttempted?.Invoke(this, new(ExportAction.ConfigureExport)
            {
                Success = false,
                MoreInfo = $"{nameof(path)} is null or empty"
            });
            return;
        }

        ExportActionResult result = await ExportService.ExportByCollectionOwners(owners, path);
        ExportCompleted?.Invoke(this, result);
    }



    public event ExportFailEventHandler? EmptyExportAttempted;
    public event ExportSuccessfullEventHandler? ExportCompleted;
}
