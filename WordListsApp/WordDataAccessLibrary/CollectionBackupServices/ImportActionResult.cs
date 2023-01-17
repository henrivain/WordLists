using System.Diagnostics.CodeAnalysis;

namespace WordDataAccessLibrary.CollectionBackupServices;
public struct ImportActionResult
{
    [SetsRequiredMembers]
    public ImportActionResult(BackupAction actionType)
    {
        ActionType = actionType;
    }

    public required BackupAction ActionType { get; init; }

    public string UsedPath { get; set; } = string.Empty;

    public bool Success { get; set; } = false;

    public string MoreInfo { get; set; } = string.Empty;
}
