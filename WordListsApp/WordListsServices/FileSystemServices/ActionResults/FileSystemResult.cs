namespace WordListsServices.FileSystemServices.ActionResults;
public class FileSystemResult : ActionResult, IFileSystemResult
{
	public FileSystemResult(bool success) : base(success) { }
	public FileSystemResult(bool success, string message) : base(success, message) { }
	
	/// <summary>
	/// Destination path for files.
	/// </summary>
	public string? OutputPath { get; set; }
	
	/// <summary>
	/// Path where input is taken from.
	/// </summary>
	public string? InputPath { get; set; }
}
