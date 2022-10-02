using WordListsMauiHelpers.Extensions;
using WordListsViewModels.Events;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WritingTestConfigurationViewModel : IWritingTestConfigurationViewModel
{

    [ObservableProperty]
    WordCollection collection = new();

    [ObservableProperty]
    bool questionsFromNativeToForeign = true;

    [ObservableProperty]
    bool saveProgression = false;

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
        StartWordCollection?.Invoke(this, new()
        {
            SaveProgression = SaveProgression,
            WordCollection = BuildCollection()
        });
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
        if (QuestionsFromNativeToForeign is false)
        {
            // Swap native language word and foreign word
            list = list.Select(x => { (x.NativeLanguageWord, x.ForeignLanguageWord) = (x.ForeignLanguageWord, x.NativeLanguageWord); return x; }).ToList();
        }
        return new()
        {
            Owner = Collection.Owner,
            WordPairs = list
        };
    }
}
