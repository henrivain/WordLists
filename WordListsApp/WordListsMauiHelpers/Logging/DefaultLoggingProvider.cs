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

    public static ILogger GetAppDefaultLogger(long? maxSizeBytes = 512_000_000)
    {
        if (LoggerConfigured is false)
        {
            var sink = new DebugEventSink();

            string filePath = GetLogFilePath();
            string? erMessage = LogFileHelper.ClearIfOverLimit(filePath, maxSizeBytes);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
#if DEBUG
                .WriteTo.Sink(sink)
#endif
                .WriteTo.File(filePath)  
                .CreateLogger();
            Log.Logger.Information("---------------------------------------------");
            Log.Logger.Information("New serilogger created in '{name}'.", nameof(DefaultLoggingProvider));
            Log.Logger.Information("Application '{name}' running version '{version}'", AppInfo.Name, VersionTracking.CurrentVersion);
            if (string.IsNullOrWhiteSpace(erMessage) is false)
            {
                // Log possible error messages for file size check
                Log.Logger.Error(erMessage);
            }
            LoggerConfigured = true;
        }
        return Log.Logger;
    }



    
}
