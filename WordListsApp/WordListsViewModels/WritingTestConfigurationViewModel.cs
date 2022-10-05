using System.Reflection.PortableExecutable;
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

    [ObservableProperty]
    bool isBusy = false;


    public event StartWordCollectionEventHandler? StartWordCollection;

    public IAsyncRelayCommand StartTestCommand => new AsyncRelayCommand(async () =>
    {
        IsBusy = true;
        StartWordCollection?.Invoke(this, new()
        {
            SaveProgression = SaveProgression,
            WordCollection = await BuildCollection()
        });
        IsBusy = false;
    });

    public async Task<WordCollection> BuildCollection()
    {
        var list = Collection.WordPairs.Shuffle();

        if (int.TryParse(SelectedPairCount, out var pairCount) is false)
        {
            pairCount = 10;
        }
        if (pairCount > -1 && pairCount <= list.Count)
        {
            list = await Task.Run(() =>
            {
                list = list.GetRange(0, pairCount);
                if (QuestionsFromNativeToForeign) return list;
                return SwapLanguages(list);
            });
        }
        return new()
        {
            Owner = Collection.Owner,
            WordPairs = list
        };

    }

    private List<WordPair> SwapLanguages(List<WordPair> list)
    {
        // Swap native language word with foreign word and swap language headers if has - as separator
        list = list.Select(x => { (x.NativeLanguageWord, x.ForeignLanguageWord) = (x.ForeignLanguageWord, x.NativeLanguageWord); return x; }).ToList();
        var headers = Collection.Owner.LanguageHeaders.Split('-');
        Array.Reverse(headers);
        Collection.Owner.LanguageHeaders = string.Join("-", headers);
        return list;
    }
}
