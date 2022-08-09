namespace WordListsViewModels.Interfaces;
public interface IWordDataViewModel
{
    public int WordListCount { get; }
    public int WordCount { get; }
    public int LearnedWordCount { get; }
    public int MightKnowWordCount { get; }
    public int NeverHeardWordCount { get; }

    public IAsyncRelayCommand UpdateData { get; }
}
