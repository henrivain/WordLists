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
			Logger.LogInformation("Get directory returns '{path}'", result);
			return true;
		}
		catch 
		{
			Logger.LogWarning("Get directory cannot read directory from '{path}'", path);
            return false;
		}
	}

	protected bool GetFileName(string? path, out string? result)
	{
		result = null;
		try
		{
			result = Path.GetFileName(path);
			Logger.LogInformation("Get file name returns '{name}'", result);
			return true;
        }
		catch 
		{
			Logger.LogWarning("Cannot get file name from '{path}'", path);
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
