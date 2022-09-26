using WordListsViewModels.Events;

namespace WordListsViewModels.Interfaces;
public interface IWritingTestConfigurationViewModel
{
    WordCollection Collection { get; set; }

    string SelectedPairCount { get; set; }

    IRelayCommand StartTestCommand { get; }

    event StartWordCollectionEventHandler StartWordCollection;
}
