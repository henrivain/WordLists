using WordListsMauiHelpers.Extensions;

namespace WordListsViewModels;
public class WritingTestConfigurationViewModel : IWritingTestConfigurationViewModel
{


    public WordCollection Collection { get; set; } = new();

    [ObservableProperty]
    int wordPairCount = -1;


    public WordCollection BuildCollection()
    {
        if (WordPairCount < 1)
        {
            WordPairCount = Collection.WordPairs.Count;
        }

        var list = Collection.WordPairs.Shuffle();
        if (WordPairCount > -1 && WordPairCount < list.Count)
        {
            list = list.GetRange(0, WordPairCount);
        }
        return new()
        {
            Owner = Collection.Owner,
            WordPairs = list
        };
    }
}
