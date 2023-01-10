using WordListsServices.Extensions;

namespace WordListsServices.FileSystemServices;
public class SafeFileHandler
{
	protected SafeFileHandler(ILogger logger) 
	{ 
		Logger = logger; 
	}

    public ILogger Logger { get; }

    protected bool TryGetDirectory(string? path, out string? result)
	{
		result = null;
		try
		{
			result = Path.GetDirectoryName(path).AddDirSeparator();
            Logger.LogInformation("{name} returns '{name}'.", nameof(TryGetDirectory), result);
            return true;
		}
		catch 
		{
			Logger.LogWarning("Get directory cannot read directory from '{path}'.", path);
            return false;
		}
	}

	protected bool TryGetFileName(string? path, out string? result)
	{
		result = null;
		try
		{
			result = Path.GetFileName(path);
            Logger.LogInformation("{name} returns '{name}'.", nameof(TryGetFileName), result);
			return true;
        }
		catch 
		{
			Logger.LogWarning("Cannot get file name from '{path}'.", path);
            return false;
		}
	}

	protected bool TryGetFileExtension(string? path, out string? result)
	{
		try
		{
			result = Path.GetExtension(path);
            Logger.LogInformation("{name} returns '{name}'.", nameof(TryGetFileExtension), result);
            return true;
		}
		catch 
		{
            result = null;
			Logger.LogWarning("Cannot get file extension from '{path}'.", path);
			return false;
        }
    }

	protected bool TryEnumerateFileNames(string? directory, out string[] fileNames)
	{
		fileNames = Array.Empty<string>();
		if (string.IsNullOrEmpty(directory))
		{
            return false;
        }

		try
		{
			fileNames = Directory.EnumerateFiles(directory).ToArray();


			return true;
		}
		catch (Exception ex) 
		{
			Logger.LogWarning("Cannot enumerate files in directory '{dir}', Exception '{ex}', '{msg}' was thrown.", 
				directory, ex.GetType().Name, ex.Message);
			return false;
		}
	}

    protected bool TryEnumerateDirectoriesNames(string? directory, out string[] directories)
    {
        directories = Array.Empty<string>();
        if (string.IsNullOrEmpty(directory))
        {
            return false;
        }

        try
        {
            directories = Directory.EnumerateDirectories(directory).ToArray();


            return true;
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Cannot enumerate directories in directory '{dir}', Exception '{ex}', '{msg}' was thrown.",
                directory, ex.GetType().Name, ex.Message);
            return false;
        }
    }
}
