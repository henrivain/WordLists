using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Interfaces;

public interface ITestResultViewModel
{
    List<WordPairQuestion> AnsweredQuestions { get; set; }
    string SessionId { get; set; }
    IRelayCommand ExitResultsCommand { get; }
    IAsyncRelayCommand SaveProgression { get; }
    int Score { get; }
    double CharMatchPercentage { get; }
    bool ProgressionSaved { get; set; }
    bool ProgressionNotSaved { get; }

    public event ExitResultsEventHandler ExitResults;
}
