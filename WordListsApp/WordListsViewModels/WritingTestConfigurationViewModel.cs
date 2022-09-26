using WordListsMauiHelpers.Extensions;
using WordListsViewModels.Events;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WritingTestConfigurationViewModel : IWritingTestConfigurationViewModel
{

    [ObservableProperty]
    WordCollection collection = new();

    string _selectedPairCount = "10";

    public string SelectedPairCount
    {
        get => _selectedPairCount;
        set
        {
            _selectedPairCount = new(value.Where(x => char.IsNumber(x)).ToArray());
            OnPropertyChanged(nameof(SelectedPairCount));
        }
    }


    public event StartWordCollectionEventHandler? StartWordCollection;

    public IRelayCommand StartTestCommand => new RelayCommand(() =>
    {
        StartWordCollection?.Invoke(this, BuildCollection());
    });

    public WordCollection BuildCollection()
    {
        var list = Collection.WordPairs.Shuffle();

        if (int.TryParse(SelectedPairCount, out var pairCount) is false)
        {
            pairCount = 10;
        }
        if (pairCount > -1 && pairCount <= list.Count)
        {
            list = list.GetRange(0, pairCount);
        }
        return new()
        {
            Owner = Collection.Owner,
            WordPairs = list
        };
    }


}
