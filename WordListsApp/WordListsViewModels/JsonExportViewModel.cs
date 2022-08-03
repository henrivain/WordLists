using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WordListsViewModels.Helpers;
using WordDataAccessLibrary.JsonServices;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class JsonExportViewModel : IJsonExportViewModel
{
    public JsonExportViewModel()
    {
        _ = ResetCollections();
    }

    

    [ObservableProperty]
    public List<WordCollectionOwner> availableCollections = new(){};

    [ObservableProperty]
    public List<object> selectedCollections = new();

    public IAsyncRelayCommand ExportSelections => new AsyncRelayCommand(async () =>
    {

    });

    public IAsyncRelayCommand ExportByName => new AsyncRelayCommand(async () =>
    {

    });

    public IAsyncRelayCommand ExportByLanguage => new AsyncRelayCommand(async () =>
    {

    });

    public IAsyncRelayCommand ExportAllShown => new AsyncRelayCommand(async () =>
    {

    });




    public async Task ResetCollections()
    {
        AvailableCollections = (await WordCollectionOwnerService.GetAll()).SortByName();
    }
}
