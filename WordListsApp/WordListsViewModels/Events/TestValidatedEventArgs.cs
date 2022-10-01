using WordListsViewModels.Helpers;

namespace WordListsViewModels.Events;

public class TestValidatedEventArgs
{
    public string SessionId { get; set; } = string.Empty;
    public List<WordPairQuestion>? Questions { get; set; }
}