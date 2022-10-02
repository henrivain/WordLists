using WordListsViewModels.Events;

namespace WordListsViewModels.Interfaces;
public interface IWritingTestConfigurationViewModel
{
    WordCollection Collection { get; set; }

    string SelectedPairCount { get; set; }
    
    bool QuestionsFromNativeToForeign { get; set; }

    bool SaveProgression { get; set; }

    IRelayCommand StartTestCommand { get; }

    event StartWordCollectionEventHandler StartWordCollection;
}
