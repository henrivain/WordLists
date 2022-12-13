namespace WordListsServices;
public interface IActionResult
{
    bool Success { get; }
    string Message { get; }
}
