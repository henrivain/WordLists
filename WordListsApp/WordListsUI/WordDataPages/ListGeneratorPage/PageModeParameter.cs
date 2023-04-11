namespace WordListsUI.WordDataPages.ListGeneratorPage;
public class PageModeParameter<T> where T : Enum
{
    public required T Mode { get; init; }
    public object? Data { get; init; }
}
