namespace WordLists.ServiceProviders;
internal class DefaultLoggingProvider
{
    static HashSet<string> UsedLogPaths { get; set; } = Enumerable.Empty<string>().ToHashSet<string>();

    /// <summary>
    /// Get path to log file that exist (created if not)
    /// </summary>
    /// <param name="logFileName"></param>
    /// <returns>path to log file that exist in file system.</returns>
    /// <exception cref="InvalidProgramException">If cannot create file.</exception>
    internal static string GetLogFilePath(string logFileName = "wordlist_runtime.log")
    {
        string directory = Path.Combine(FileSystem.AppDataDirectory, "Logs");
        string file = Path.Combine(directory, logFileName);
        try
        {
            if (Directory.Exists(directory) is false)
            {
                Directory.CreateDirectory(directory);
            }
            if (File.Exists(file) is false)
            {
                File.Create(file).Close();
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Cannot create new log file.", ex);
        }
        UsedLogPaths.Add(file);
        return file;
    }
}
