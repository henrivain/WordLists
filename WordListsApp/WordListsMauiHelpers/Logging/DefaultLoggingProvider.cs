using Serilog;
namespace WordListsMauiHelpers.Logging;
public class DefaultLoggingProvider : ILoggingInfoProvider
{
    static HashSet<string> UsedLogPaths { get; set; } = Enumerable.Empty<string>().ToHashSet();

#if DEBUG
    const string DefaultLogFileName = "wordlist_runtime_debug.log";
#else
    const string DefaultLogFileName = "wordlist_runtime.log";
#endif

    /// <summary>
    /// Get path to log file that exist (created if not)
    /// </summary>
    /// <param name="logFileName"></param>
    /// <returns>path to log file that exist in file system.</returns>
    /// <exception cref="InvalidOperationException">If cannot create file.</exception>
    public static string GetLogFilePath(string logFileName = DefaultLogFileName)
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



    public static bool LoggerConfigured { get; set; } = false;

    public string[] LoggingFilePaths => UsedLogPaths.ToArray();

    public static ILogger GetFileLogger()
    {
        if (LoggerConfigured is false)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(GetLogFilePath())  //fileSizeLimitBytes: 10_000_000 ??
                .CreateLogger();
            Log.Logger.Information("New serilogger created in '{name}'.", nameof(DefaultLoggingProvider));
            LoggerConfigured = true;
        }
        return Log.Logger;
    }


}
