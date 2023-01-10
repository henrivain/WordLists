using Microsoft.Extensions.Logging;
using WordListsServices.FileSystemServices.ActionResults;

namespace WordListsServices.FileSystemServices;
public class FolderHandler : IFolderHandler
{
    public FolderHandler(ILogger logger)
    {
        Logger = logger;
    }

    public ILogger Logger { get; }

    /// <inheritdoc/>
    public FileSystemResult CreateDirectory(string? path)
    {
        Logger.LogInformation("Create directory to path {path}", path);
        if (Path.Exists(path))
        {
            return new(true)
            {
                Message = "Directory already exist",
                OutputPath = path,
            };
        }
        if (string.IsNullOrWhiteSpace(path))
        {
            return new(false)
            {
                Message = "Cannot create directory to empty path"
            };
        }
        try
        {
            Directory.CreateDirectory(path);   
            return new(true) { OutputPath = path };
        }
        catch (Exception ex)
        {
            string msg = ex switch
            {
                DirectoryNotFoundException => "Part of path is invalid.",
                PathTooLongException => "Cannot create dictioanry with too long path.",
                IOException => "Cannot create directory that is already a file.",
                UnauthorizedAccessException => "No permission to create directory.",
                ArgumentException => "Cannot create dictioanry with invalid path format.",
                NotSupportedException => "Path has colon (:) that is not a part of drive letter.",
                _ => $"Unknown exception at {nameof(FolderHandler)}.{nameof(CreateDirectory)}. " +
                $"'{ex.GetType().Name}', '{ex.Message}'"
            };
            Logger.LogWarning("Failed to create dictionary, because of '{ex}', '{msg}'", 
                ex.GetType().Name, ex.Message);
            return new(false) 
            {
                Message = msg,
                OutputPath = path 
            };

        }
    }
}
