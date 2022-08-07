namespace WordDataAccessLibrary.CollectionBackupServices.ActionResults;

public struct FileActionResult
{
    public FileActionResult(FileAction action)
    {
        FileAction = action;
    }
    public bool Success { get; set; } = false;
    public FileAction FileAction { get; set; } = FileAction.NotSet;
    public string UsedPath { get; set; } = string.Empty;
    public string MoreInfo { get; set; } = string.Empty;
}