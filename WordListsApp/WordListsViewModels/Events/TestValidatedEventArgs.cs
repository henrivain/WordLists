using WordListsViewModels.Helpers;

namespace WordListsViewModels.Events;

public class TestValidatedEventArgs : EventArgs
{
    public string SessionId { get; set; } = string.Empty;
    public List<WordPairQuestion>? Questions { get; set; }

    public bool ProgressionSaved { get; set; } = false;
}