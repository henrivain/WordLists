using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WordListsServices.Extensions;
using WordListsServices.FileSystemServices.ActionResults;

namespace WordListsServices.FileSystemServices;
public class FileHandler : IFileHandler
{
    public FileHandler(IFolderHandler folderHandler) : this(folderHandler, NullLogger.Instance)
    {
        FolderHandler = folderHandler;
    }

    public FileHandler(IFolderHandler folderHandler, ILogger logger)
    {
        FolderHandler = folderHandler;
        Logger = logger;
    }

    public IFolderHandler FolderHandler { get; }
    public ILogger Logger { get; }

    /// <inheritdoc/>
    public async Task<FileSystemResult> CopyFileAsync(string? inputFile, string? destinationFolder, bool overwrite = true)
    {
        return await Task.Run(() => CopyFile(inputFile, destinationFolder, overwrite));
    }

    /// <inheritdoc/>
    public FileSystemResult CopyFile(string? inputFile, string? destinationFolder, bool overwrite = true)
    {
        Logger.LogInformation("Copy file from '{input}' to '{output}'", inputFile, destinationFolder);
        string? fileName = Path.GetFileName(inputFile);

        if (File.Exists(inputFile) is false)
        {
            Logger.LogWarning("Cannot copy file that doesn't exist.");
            return new(false)
            {
                Message = "Input file doesn't exist",
                InputPath = inputFile,
                OutputPath = destinationFolder,
            };
        }
        if (string.IsNullOrWhiteSpace(fileName))
        {
            Logger.LogWarning("Cannot copy file without name.");
            return new(false)
            {
                Message = $"Input file '{inputFile}' doesn't have file name",
                InputPath = inputFile,
                OutputPath = destinationFolder,
            };
        }
        if (string.IsNullOrWhiteSpace(destinationFolder))
        {
            Logger.LogWarning("Cannot copy file without destination folder.");
            return new(false)
            {
                Message = "Destination folder is null",
                InputPath = inputFile,
                OutputPath = destinationFolder,
            };
        }

        string destinationPath = Path.Combine(destinationFolder, fileName);
        var folderCreated = FolderHandler.CreateDirectory(destinationFolder);
        if (folderCreated.NotSuccess())
        {
            Logger.LogWarning("Cannot copy file, because destination folder cannot be created.");
            return folderCreated;
        }
        try
        {
            File.Copy(inputFile, destinationPath, overwrite);
            Logger.LogInformation("File created successfully");
            return new(true)
            {
                OutputPath = destinationPath,
                InputPath = inputFile
            };
        }
        catch (Exception ex) 
        {
            string msg = ex switch
            {
                UnauthorizedAccessException => $"No permission to copy files.",
                ArgumentException => $"Cannot copy, invalid path.",
                PathTooLongException => $"At least one path too long.",
                DirectoryNotFoundException => $"Cannot directory for source or destination.",
                FileNotFoundException => $"Cannot copy file, that doens't exist.",
                IOException => $"Cannot copy file, I/O error.",
                NotSupportedException => $"Invalid path format.",
                _ => $"Unknown exception in {nameof(FileHandler)}.{nameof(CopyFile)}" +
                $"Exception '{ex.GetType()}', '{ex.Message}'."
            };
            Logger.LogWarning("Failed to copy file because of '{ex}', '{msg}'", ex.GetType().Name, ex.Message);  
            return new(false)
            {
                Message = msg,
                OutputPath = destinationFolder,
                InputPath = inputFile
            };
        }
    }

    /// <inheritdoc/>
    public Task<FileSystemResult> CopyMatchingFilesTo(string? inputDir, string? outputDir, params string[] nameArgs)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public FileSystemResult Create(string? filePath)
    {
        Logger.LogInformation("Try to create file at {path}", filePath);
        string? dir;
        try
        {
            dir = Path.GetDirectoryName(filePath);
        }
        catch (PathTooLongException)
        {
            Logger.LogWarning("Cannot create file, path too long.");
            return new(false)
            {
                Message = $"Cannot create file path '{filePath}', because it is too long.",
                OutputPath = filePath
            };
        }
        if (string.IsNullOrWhiteSpace(dir))
        {

            Logger.LogWarning("Cannot create file, no directory in path.", filePath);
            return new(false)
            {
                Message = $"Cannot get direcotry name from path '{filePath}'",
                OutputPath = filePath
            };
        }
        if (File.Exists(filePath))
        {
            Logger.LogWarning("File alredy exist.");
            return new(true) { OutputPath = filePath };
        }
        var folderCreated = FolderHandler.CreateDirectory(dir);
        if (folderCreated.NotSuccess())
        {
            Logger.LogWarning("Failed to create file. '{msg}'.", folderCreated.Message);
            return folderCreated;
        }
        try
        {
            File.Create(filePath!).Close();  // Cannot be null if can get directory
            Logger.LogInformation("File created successfully.");
            return new(true) { OutputPath = filePath };
        }
        catch (Exception ex)
        {
            string msg = ex switch
            {
                UnauthorizedAccessException => $"No permission to create file at {filePath}.",
                ArgumentException => $"Cannot create file with invalid path '{filePath}'.",
                PathTooLongException => $"Too long path to be created '{filePath}'.",
                DirectoryNotFoundException => $"Cannot create file '{filePath}'. No directory exist.",
                IOException => $"I/O exception whilst creating file '{filePath}'.",
                NotSupportedException => $"Cannot create file '{filePath}' invalid path format.",
                _ => $"Unknown exception in {nameof(FileHandler)}.{nameof(Create)}. " +
                $"Exception '{ex.GetType()}', '{ex.Message}'."
            };
            Logger.LogWarning("Failed to create file because of '{ex}', '{msg}'", ex.GetType().Name, ex.Message);

            return new(false)
            {
                Message = msg,
                OutputPath = filePath
            };
        }
    }

    /// <inheritdoc/>
    public async Task<FileSystemResult> DeleteAsync(string? filePath)
    {
        return await Task.Run(() => Delete(filePath));
    }

    /// <inheritdoc/>
    public FileSystemResult Delete(string? filePath)
    {
        Logger.LogInformation("Try delete file at {path}", filePath);
        if (File.Exists(filePath) is false)
        {
            Logger.LogInformation("Success, no file found");
            return new(true)
            {
                Message = "Success, no file was found.",
                OutputPath = filePath
            };
        }
        try
        {
            File.Delete(filePath);
            Logger.LogInformation("Deleted successfully.");
            return new FileSystemResult(true) { OutputPath = filePath };
        }
        catch (Exception ex)
        {
            string msg = ex switch
            {
                ArgumentException => $"Cannot delete file with invalid path.",
                DirectoryNotFoundException => $"Cannot find directory for file to be deleted.",
                PathTooLongException => $"Cannot delete file with too long path.",
                IOException => $"I/O exception while trying to delete file.",
                NotSupportedException => $"Cannot delete file with invalid path format.",
                UnauthorizedAccessException => $"Lacking permission to delete file.",
                _ => $"Unknown Exception at {nameof(FileHandler)}.{nameof(Delete)}. " +
                    $"Exception '{ex.GetType()}', '{ex.Message}'"
            };
            Logger.LogWarning("Failed to delete file because of '{ex}', '{msg}'", ex.GetType().Name, ex.Message);

            return new FileSystemResult(false)
            {
                Message = msg,
                OutputPath = filePath
            };
        }
    }
}
