namespace WordListsViewModels.Events;

public class TestStartEventArgs : EventArgs
{
    public WordCollection? WordCollection { get; set; }
    public bool SaveProgression { get; set; } = false;
}