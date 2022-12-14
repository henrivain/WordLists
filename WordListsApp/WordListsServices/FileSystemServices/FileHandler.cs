using WordListsServices.Extensions;
using WordListsServices.FileSystemServices.ActionResults;

namespace WordListsServices.FileSystemServices;
public class FileHandler : SafeFileHandler, IFileHandler
{
    public FileHandler(IFolderHandler folderHandler) : this(folderHandler, NullLogger.Instance)
    {
        FolderHandler = folderHandler;
    }

    public FileHandler(IFolderHandler folderHandler, ILogger logger) : base(logger)
    {
        FolderHandler = folderHandler;
    }

    public IFolderHandler FolderHandler { get; }

    /// <inheritdoc/>
    public async Task<IFileSystemResult> CopyFileAsync(
        string? inputFile, string? destinationFolder, bool overwrite = true)
    {
        PermissionHandler handler = new(Logger);
        if (await handler.RequestFileSystemReadAccess() is false)
        {
            Logger.LogWarning("Cannot get permission to read file system.");
            return new FileSystemResult(false)
            {
                InputPath = inputFile,
                OutputPath = destinationFolder,
                Message = "Cannot get permission to use file system to read files."
            };
        }
        if (await handler.RequestFileSystemWriteAccess() is false)
        {
            Logger.LogWarning("Cannot get permission to write to file system.");
            return new FileSystemResult(false)
            {
                InputPath = inputFile,
                OutputPath = destinationFolder,
                Message = "Cannot get permission to use file system to write files."
            };
        }
        return await Task.Run(() => CopyFile(inputFile, destinationFolder, overwrite));
    }

    /// <inheritdoc/>
    private IFileSystemResult CopyFile(
        string? inputFile, string? destinationFolder, bool overwrite = true)
    {
        Logger.LogInformation("Copy file from '{input}' to '{output}'", inputFile, destinationFolder);
        string? fileName = Path.GetFileName(inputFile);

        if (File.Exists(inputFile) is false)
        {
            Logger.LogWarning("Cannot copy file that doesn't exist.");
            return new FileSystemResult(false)
            {
                Message = "Input file doesn't exist",
                InputPath = inputFile,
                OutputPath = destinationFolder,
            };
        }
        if (string.IsNullOrWhiteSpace(fileName))
        {
            Logger.LogWarning("Cannot copy file without name.");
            return new FileSystemResult(false)
            {
                Message = $"Input file '{inputFile}' doesn't have file name",
                InputPath = inputFile,
                OutputPath = destinationFolder,
            };
        }
        if (string.IsNullOrWhiteSpace(destinationFolder))
        {
            Logger.LogWarning("Cannot copy file without destination folder.");
            return new FileSystemResult(false)
            {
                Message = "Destination folder is null",
                InputPath = inputFile,
                OutputPath = destinationFolder,
            };
        }

        // Have to use some other way in android


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
            return new FileSystemResult(true)
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
            return new FileSystemResult(false)
            {
                Message = msg,
                OutputPath = destinationFolder,
                InputPath = inputFile
            };
        }
    }

    /// <inheritdoc/>
    public async Task<IFileCopyResult> CopyMatchingFilesAsync(
        string? inputDir, string? outputDir, params string[] nameArgs)
    {
        Logger.LogInformation("Copy files that have words '{args}' in their file name, from '{inputPath}' to '{outputPath}'.",
            string.Join(", ", nameArgs), inputDir, outputDir);
        
        if (TryGetDirectory(inputDir, out inputDir) is false)
        {
            Logger.LogWarning("Failed to copy files, input path was invalid.");
            return new FileCopyResult(false)
            {
                Message = "Input directory was invalid.",
                InputPath = inputDir,
                OutputPath = outputDir
            };
        }
        if (TryGetDirectory(outputDir, out outputDir) is false)
        {
            Logger.LogWarning("Failed to copy files, output path was invalid.");
            return new FileCopyResult(false)
            {
                Message = "Output directory was invalid.",
                InputPath = inputDir,
                OutputPath = outputDir
            };
        }
        if(TryEnumerateFileNames(inputDir, out var filePaths) is false)
        {
            Logger.LogWarning("Failed to copy files, cannot enumerate file system entries.");
            return new FileCopyResult(false)
            {
                Message = "Cannot enumerate files in input directory.",
                InputPath = inputDir,
                OutputPath = outputDir
            };
        }

        string[] validEntries = filePaths.Where(path => NameFilter(path, nameArgs)).ToArray();
        if (validEntries.Length <= 0)
        {
            Logger.LogWarning("Cannot copy files, because none was found that fould match params: '{params}'.",
                string.Join(", ", nameArgs));
            return new FileCopyResult(true)
            {
                Message = $"Success? No files were found that could match given parameters: '{string.Join(", ", nameArgs)}'.",
                InputPath = inputDir,
                OutputPath = outputDir
            };
        }

        int failedCopies = 0;
        int validCopies = 0;
        foreach (string entry in validEntries)
        {
            var actionResult = await CopyFileAsync(entry, outputDir);
            if (actionResult.NotSuccess())
            {
                failedCopies ++;
                continue;
            }
            validCopies ++;
        }
        if (failedCopies > 0)
        {
            Logger.LogWarning("'{failed}', out of '{total}' copies failed.", failedCopies, validEntries.Length);
            return new FileCopyResult(false)
            {
                Message = $"Failed to copy '{failedCopies}' files, '{validCopies}' files succeeded. " +
                    $"See logs for more information.",
                InputPath = inputDir,
                OutputPath = outputDir
            };
        }
        Logger.LogInformation("Successfully copied '{count}' files.", validCopies);
        return new FileCopyResult(true)
        {
            Message = $"Copied '{validCopies}' files successfully",
            InputPath = inputDir,
            OutputPath = outputDir
        };
    }



    /// <inheritdoc/>
    public IFileSystemResult Create(string? filePath)
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
            return new FileSystemResult(false)
            {
                Message = $"Cannot create file path '{filePath}', because it is too long.",
                OutputPath = filePath
            };
        }
        if (string.IsNullOrWhiteSpace(dir))
        {

            Logger.LogWarning("Cannot create file, no directory in path.", filePath);
            return new FileSystemResult(false)
            {
                Message = $"Cannot get direcotry name from path '{filePath}'",
                OutputPath = filePath
            };
        }
        if (File.Exists(filePath))
        {
            Logger.LogWarning("File alredy exist.");
            return new FileSystemResult(true) { OutputPath = filePath };
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
            return new FileSystemResult(true) { OutputPath = filePath };
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

            return new FileSystemResult(false)
            {
                Message = msg,
                OutputPath = filePath
            };
        }
    }

    /// <inheritdoc/>
    public async Task<IFileSystemResult> DeleteAsync(string? filePath)
    {
        return await Task.Run(() => Delete(filePath));
    }

    /// <inheritdoc/>
    public IFileSystemResult Delete(string? filePath)
    {
        Logger.LogInformation("Try delete file at {path}", filePath);
        if (File.Exists(filePath) is false)
        {
            Logger.LogInformation("Success, no file found");
            return new FileSystemResult(true)
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





    private bool NameFilter(string path, string[] nameArgs)
    {
        if (GetFileName(path, out string? name) is false)
        {
            return false;
        }
        if (name is null)
        {
            return false;
        }
        foreach (var arg in nameArgs)
        {
            if (name.Contains(arg) is false)
            {
                return false;
            }
        }
        return true;
    }

}
