
using WordDataAccessLibrary.DataBaseActions.Interfaces;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class WordDataViewModel : IWordDataViewModel
{
    public WordDataViewModel(IWordCollectionService collectionService, IWordPairService pairService)
    {
        CollectionService = collectionService;
        PairService = pairService;
        UpdateData.Execute(null);
    }

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

    [ObservableProperty]
    int notSetWordCount = 0;

    public IAsyncRelayCommand UpdateData => new AsyncRelayCommand(async () =>
    {
        WordListCount = await CollectionService.CountItems();
        WordCount = await PairService.CountItems();
        LearnedWordCount = await PairService.CountItemsMatching(x => x.LearnState == WordLearnState.Learned);
        MightKnowWordCount = await PairService.CountItemsMatching(x => x.LearnState == WordLearnState.MightKnow);
        NeverHeardWordCount = await PairService.CountItemsMatching(x => x.LearnState == WordLearnState.NeverHeard);
        NotSetWordCount = await PairService.CountItemsMatching(x => x.LearnState == WordLearnState.NotSet);
    });

    IWordCollectionService CollectionService { get; }
    public IWordPairService PairService { get; }
}
