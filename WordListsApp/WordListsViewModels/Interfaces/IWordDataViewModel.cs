namespace WordListsViewModels.Interfaces;
public interface IWordDataViewModel
{
    int WordListCount { get; }
    int WordCount { get; }
    int LearnedWordCount { get; }
    int MightKnowWordCount { get; }
    int NeverHeardWordCount { get; }
    int NotSetWordCount { get; }

    IAsyncRelayCommand UpdateData { get; }
}
