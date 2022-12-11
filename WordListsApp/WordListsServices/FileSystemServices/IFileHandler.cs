using WordListsServices.FileSystemServices.ActionResults;

namespace WordListsServices.FileSystemServices;

/// <summary>
/// Actions for handling files (Throws no exceptions)
/// </summary>
public interface IFileHandler
{
    /// <summary>
    /// Copy file from given path to destination path (that is created if doesn't exist)
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="destinationFile"></param>
    /// <returns>FileSystemResult where OutputPath => destinationFile, InputPath => inputFile.</returns>
    FileSystemResult CopyFile(string? inputFile, string? destinationFile, bool overwrite = true);

    /// <summary>
    /// Copy file from given path to destination path (that is created if doesn't exist) async.
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="destinationFile"></param>
    /// <returns>FileSystemResult where OutputPath => destinationFile, InputPath => inputFile.</returns>
    Task<FileSystemResult> CopyFileAsync(string? inputFile, string? destinationFile, bool overwrite = true);

    /// <summary>
    /// Delete file asynchronously from given path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>FileSystemResult where OutputPath => filePath.</returns>
    Task<FileSystemResult> DeleteAsync(string? filePath);

    /// <summary>
    /// Delete file from given path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>FileSystemResult where OutputPath => filePath.</returns>
    FileSystemResult Delete(string? filePath);

    /// <summary>
    /// Create file to given directory. Also create any directories in the path that doens't exist.
    /// </summary>
    /// <param name="filePath">File path to be created.</param>
    /// <returns>FileSystemResult where OutputPath => filePath.</returns>
    FileSystemResult Create(string? filePath);

    /// <summary>
    /// Copy all files that contain all given substrings (nameArgs) in its name.
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir"></param>
    /// <param name="nameArgs"></param>
    /// <returns>FileSystemResult where InputPath => inputDir, OutputPath => outputDir</returns>
    Task<FileSystemResult> CopyMatchingFilesTo(string? inputDir, string? outputDir, params string[] nameArgs);
}
