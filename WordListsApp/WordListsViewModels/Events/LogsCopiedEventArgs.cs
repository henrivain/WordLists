using System.Diagnostics.CodeAnalysis;

namespace WordListsViewModels.Events;

public class LogsCopiedEventArgs
{
    public LogsCopiedEventArgs() { }

    [SetsRequiredMembers]
	public LogsCopiedEventArgs(bool success)
	{
        Success = success;
    }

    public required bool Success { get; init; }
    public string? Message { get; set; }
    public string? OutputDirectory { get; set; }
    public int FilesValid { get; set; }
    public int FilesFailed { get; set; }
}