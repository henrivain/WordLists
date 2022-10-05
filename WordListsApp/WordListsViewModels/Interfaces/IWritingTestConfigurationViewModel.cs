using WordListsViewModels.Events;

namespace WordListsViewModels.Interfaces;
public interface IWritingTestConfigurationViewModel
{
    WordCollection Collection { get; set; }

    string SelectedPairCount { get; set; }
    
    bool QuestionsFromNativeToForeign { get; set; }

    bool IsBusy { get; }

    bool SaveProgression { get; set; }

    IAsyncRelayCommand StartTestCommand { get; }

    event StartWordCollectionEventHandler StartWordCollection;
}
