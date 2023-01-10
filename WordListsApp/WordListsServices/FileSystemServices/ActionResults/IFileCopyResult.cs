namespace WordListsServices.FileSystemServices.ActionResults;
public interface IFileCopyResult : IFileSystemResult
{
    public int FilesCopiedSuccessfully { get; }
    public int FilesFailed { get; }
}
