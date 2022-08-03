using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class JsonExportViewModel : IJsonExportViewModel
{
    public JsonExportViewModel()
    {

    }

    [ObservableProperty]
    public List<WordCollection> availableWordCollections = new()
    {
        new()
        {
            Owner = new()
            {
                Name = "jtoots"
            }
        },
        new()
        {
            Owner = new()
            {
                Name = "jönks"
            }
        }
    };

    [ObservableProperty]
    public List<WordCollection> selectedCollections = new();

    public IAsyncRelayCommand ExportSelections => throw new NotImplementedException();

    public IAsyncRelayCommand ExportByName => throw new NotImplementedException();

    public IAsyncRelayCommand ExportByLanguage => throw new NotImplementedException();

    public IAsyncRelayCommand ExportByAll => throw new NotImplementedException();
}
