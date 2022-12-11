using WordListsServices.FileSystemServices.ActionResults;

namespace WordListsServices.FileSystemServices;
public interface IFolderHandler
{
    /// <summary>
    /// Create directory to full path if it doens't exit already.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>FileSystemResult representing information about action and action status</returns>
    FileSystemResult CreateDirectory(string? path);





}
