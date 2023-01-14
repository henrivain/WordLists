namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordListViewModel : IWordListViewModel
{
    [ObservableProperty]
    WordCollection _collection = new();
}
