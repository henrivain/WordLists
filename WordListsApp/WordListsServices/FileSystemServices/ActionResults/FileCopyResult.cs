namespace WordListsServices.FileSystemServices.ActionResults;
public class FileCopyResult : FileSystemResult, IFileCopyResult
{
    public FileCopyResult(bool success) : base(success) { }
    public FileCopyResult(bool success, string message) : base(success, message) { }
    public int FilesCopiedSuccessfully { get; set; }
    public int FilesFailed { get; set; }
}
