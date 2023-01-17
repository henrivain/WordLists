using System.Diagnostics.CodeAnalysis;

namespace WordDataAccessLibrary.CollectionBackupServices.ActionResults;

public struct FileActionResult
{
    public FileActionResult() { }

    [SetsRequiredMembers]
    public FileActionResult(FileAction action)
    {
        FileAction = action;
    }

    public bool Success { get; set; } = false;
    public required FileAction FileAction { get; set; }
    public string UsedPath { get; set; } = string.Empty;
    public string MoreInfo { get; set; } = string.Empty;
}