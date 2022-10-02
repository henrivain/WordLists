using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Interfaces;

public interface ITestResultViewModel
{
    List<WordPairQuestion> AnsweredQuestions { get; set; }
    string SessionId { get; set; }
    IRelayCommand ExitResultsCommand { get; }
    int Score { get; }
    double CharMatchPercentage { get; }
    bool ProgressionSaved { get; set; }

    public event ExitResultsEventHandler ExitResults;
}
