using WordValidationLibrary;

namespace WordListsViewModels.Interfaces;
public interface IWriteWordViewModel
{
    string Title { get; set; }
    string Description { get; set; }
    string LanguageHeaders { get; set; }
    string UserInput { get; set; }

    WordCollection WordCollection { get; set; }
    WordPair? VisibleWordPair { get; set; }
    WordMatchResult? ValidationResult { get; }

    IRelayCommand ValidateWord { get; }
    IAsyncRelayCommand SaveProgression { get; }
    IRelayCommand RestartCommand { get; }


    bool CanGoNext { get; }
    bool CanGoPrevious { get; }
    bool ProgressSaved { get; }
    bool IsListCompleted { get; }
    bool IsEmptyCollection { get; }

    int WordIndex { get; }
    int MaxWordIndex { get; set; }


    IRelayCommand GoNext { get; }
    IRelayCommand GoPrevious { get; }

    void StartNew(WordCollection collection);
    void StartNew(WordCollection collection, int fromIndex);
    Task StartNewAsync(int collectionId);
}
