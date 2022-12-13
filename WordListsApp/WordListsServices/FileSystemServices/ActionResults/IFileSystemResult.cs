namespace WordListsServices.FileSystemServices.ActionResults;
public interface IFileSystemResult : IActionResult
{
    string? OutputPath { get; }

    string? InputPath { get; }
}
