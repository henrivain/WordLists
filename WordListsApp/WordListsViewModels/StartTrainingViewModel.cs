using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class StartTrainingViewModel
{
    public List<WordCollectionOwner> AvailableCollections { get; private set; } = new();

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

    public async Task ResetCollections()
    {
        AvailableCollections = await WordCollectionOwnerService.GetAll();
    }
}
