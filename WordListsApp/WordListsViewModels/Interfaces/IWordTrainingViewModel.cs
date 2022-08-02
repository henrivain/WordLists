namespace WordListsViewModels.Interfaces;


public interface IWordTrainingViewModel
{
    string Title { get; set; }
    string Description { get; set; }
    string LanguageHeaders { get; set; }
    WordCollection WordCollection { get; set; }
    WordPair VisibleWordPair { get; set; }

    int WordIndex { get; }
    int MaxWordIndex { get; set; }
    int LearnStateAsInt { get; set; }

    bool CanGoNext { get; set; }
    bool CanGoPrevious { get; set; }

    IAsyncRelayCommand SaveProgression { get; }

    IRelayCommand WordStateNotSetCommand { get; }
    IRelayCommand WordLearnedCommand { get; }
    IRelayCommand MightKnowWordCommand { get; }
    IRelayCommand WordNeverHeardCommand { get; }

    void Next();
    void Previous();

    void StartNew(WordCollection collection);
    void StartNew(WordCollection collection, int fromIndex);
    Task StartNewAsync(int collectionId);

    public event CollectionUpdatedEventHandler? CollectionUpdated;

}
