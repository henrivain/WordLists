using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class StartTrainingViewModel : IStartTrainingViewModel
{
    [ObservableProperty]
    List<WordCollectionOwner> availableCollections = new();

    [ObservableProperty]
    string dataParameter = string.Empty;

    public IAsyncRelayCommand UpdateCollectionsByName => new AsyncRelayCommand(async () =>
    {
        AvailableCollections = await WordCollectionOwnerService.GetByName(DataParameter);
    });    
    
    public IAsyncRelayCommand UpdateCollectionsByLanguage => new AsyncRelayCommand(async () =>
    {
        AvailableCollections = await WordCollectionOwnerService.GetByLanguage(DataParameter);
    });

    public IAsyncRelayCommand UpdateCollections => new AsyncRelayCommand(async () =>
    {
        await ResetCollections();
    });

    [ObservableProperty]
    bool showLearnedWords = true;

    [ObservableProperty]
    bool showMightKnowWords = true;

    [ObservableProperty]
    bool showNeverHeardKnowWords = true;

    [ObservableProperty]
    int showWords = 1;

    [ObservableProperty]
    bool removeLearnedWords;

    [ObservableProperty]
    bool removeMightKnowWords;

    [ObservableProperty]
    bool removeNeverHeardWords;

    [ObservableProperty]
    bool isRefreshing = false;

    [ObservableProperty]
    WordCollectionOwner selectedItem = new();
    



    public WordCollection CreateCollection()
    {
        throw new NotImplementedException();
    }

    public async Task ResetCollections()
    {
        IsRefreshing = true;   
        AvailableCollections = await WordCollectionOwnerService.GetAll();
        IsRefreshing = false;
    }
}
