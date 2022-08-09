
namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordDataViewModel : IWordDataViewModel
{
    [ObservableProperty]
    int wordListCount = 0;

    [ObservableProperty]
    int wordCount = 0;
    
    [ObservableProperty]
    int learnedWordCount = 0;

    [ObservableProperty]
    int neverHeardWordCount = 0;

    [ObservableProperty]
    int mightKnowWordCount = 0;

    public IAsyncRelayCommand UpdateData => new AsyncRelayCommand(async () =>
    {
        await Task.Delay(1);
    });


}
