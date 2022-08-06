namespace WordDataAccessLibrary.CollectionBackupServices;

public struct ExportActionResult
{
    public ExportActionResult(ExportAction actionType)
    {
        ActionType = actionType;
    }

    public ExportAction ActionType { get; }

    public string UsedPath { get; set; } = string.Empty;

    public bool Success { get; set; } = false;

    public string MoreInfo { get; set; } = string.Empty;
}