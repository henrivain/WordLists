using WordListsServices.FileSystemServices.ActionResults;

namespace WordListsServices.FileSystemServices;

/// <summary>
/// Actions for handling files (Throws no exceptions)
/// </summary>
public interface IFileHandler 
{

    /// <summary>
    /// Copy file from given path to destination folder (that is created if doesn't exist) async.
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="destinationFolder"></param>
    /// <param name="overwrite"></param>
    /// <returns>FileSystemResult where OutputPath => destinationFile, InputPath => inputFile.</returns>
    Task<IFileSystemResult> CopyFileAsync(string? inputFile, string? destinationFolder, string? outputFileName = null, bool overwrite = true);

    /// <summary>
    /// Delete file asynchronously from given path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>FileSystemResult where OutputPath => filePath.</returns>
    Task<IFileSystemResult> DeleteAsync(string? filePath);

    /// <summary>
    /// Delete file from given path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>FileSystemResult where OutputPath => filePath.</returns>
    IFileSystemResult Delete(string? filePath);

    /// <summary>
    /// Create file to given directory. Also create any directories in the path that doens't exist.
    /// </summary>
    /// <param name="filePath">File path to be created.</param>
    /// <returns>FileSystemResult where OutputPath => filePath.</returns>
    IFileSystemResult Create(string? filePath);

    /// <summary>
    /// Copy all files that contain all given substrings (nameArgs) in its name. 
    /// All directory paths must end in (alt)directory separator char.
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir"></param>
    /// <param name="nameArgs"></param>
    /// <returns>FileSystemResult where InputPath => inputDir, OutputPath => outputDir</returns>
    Task<IFileCopyResult> CopyMatchingFilesAsync(string? inputDir, string? outputDir, params string[] nameArgs);
}
