namespace WordDataAccessLibrary.CollectionBackupServices;

public struct ExportActionResult
{
    public ExportActionResult(BackupAction actionType)
    {
        ActionType = actionType;
    }

    public BackupAction ActionType { get; }

    public string UsedPath { get; set; } = string.Empty;

    public bool Success { get; set; } = false;

    public string MoreInfo { get; set; } = string.Empty;
}