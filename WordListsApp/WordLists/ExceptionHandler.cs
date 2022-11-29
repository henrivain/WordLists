using Serilog;
using System.Runtime.ExceptionServices;
using Exception = System.Exception;

#nullable enable

namespace WordLists;
internal class ExceptionHandler
{

    enum LogConfiguration
    {
        File
    }

    HashSet<LogConfiguration> UsedLogConfigurations { get; } = new();

    public ExceptionHandler(AppDomain domain)
    {
        Domain = domain;
    }

    AppDomain Domain { get; }

    ILogger Logger { get; set; } = Log.Logger;

    string LogFilePath { get; set; } = GetDefaultFilePath();

    private static string GetDefaultFilePath()
    {
        string appDataPath = FileSystem.Current.AppDataDirectory;
#if WINDOWS
        appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
#endif
        return Path.Combine(appDataPath, nameof(WordLists), "runtime.log");
    }

    internal ExceptionHandler AddExceptionHandling()
    {
        Domain.FirstChanceException += FirstChanceException;
        Domain.UnhandledException += UnhandledException;
        return this;
    }
    internal ExceptionHandler LogToFile()
    {
        if (UsedLogConfigurations.Add(LogConfiguration.File) is false)
        {
            return this;
        }

        if (LogFileExist() is false)
        {
#if DEBUG
            throw new InvalidOperationException(nameof(LogFileExist));
#else
            return this;
#endif
        }
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(LogFilePath, fileSizeLimitBytes: 10_000_000)
            .CreateLogger();
        Logger = Log.Logger;

        Log.Logger.Information(LogFilePath);

        Logger.Information("App started");
        Logger.Information("New logger initialized successfully!");

        return this;
    }
    internal string GetLogFilePath() => LogFilePath;


    internal ExceptionHandler HandleWindowsExceptions()
    {
#if WINDOWS
        Microsoft.UI.Xaml.Application.Current.UnhandledException += (sender, args) =>
        {
            Exception ex = args.Exception;
            string exType = ex.GetType().Name;
            string exMessage = ex.Message;
            string trace = ex.StackTrace ?? "NULL";
            LogException(sender, exType, exMessage, trace, "This exception was caught as windows exception");
        };
#endif
        return this;
    }
    internal ExceptionHandler HandleAndroidException()
    {
#if ANDROID
        Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
        {
            Exception ex = args.Exception;
            string exType = ex.GetType().Name;
            string exMessage = ex.Message;
            string trace = ex.StackTrace ?? "NULL";
            LogException(sender, exType, exMessage, trace, additionalInfo: "This exception was caught as Android exception");
        };
#endif
        return this;
    }
    private void FirstChanceException(object? sender, FirstChanceExceptionEventArgs e)
    {
        Exception ex = e.Exception;
        string exType = ex.GetType().Name;
        string exMessage = ex.Message;
        string trace = ex.StackTrace ?? "NULL";

        LogException(sender, exType, exMessage, trace, $"This exception was caught by {nameof(FirstChanceException)}");
    }
    private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        string exType;
        string exMessage;
        string trace;
        if (e.ExceptionObject is Exception ex)
        {
            exType = ex.GetType().Name;
            exMessage = ex.Message;
            trace = ex.StackTrace ?? "NULL";
        }
        else
        {

            exType = e.ExceptionObject.GetType().Name;
            exMessage = "NULL, cant get real exception from ExceptionObject";
            trace = "NULL";
        }

        LogException(sender, exType, exMessage, trace, $"This exception was caught by {nameof(UnhandledException)}");
    }




    private void LogException(object? sender, string exType, string exMessage, string trace, string? additionalInfo = null)
    {
        string senderTypeName = sender?.GetType().Name ?? "NULL";

        if (additionalInfo is null)
        {
            Logger.Error("""
                {senderTypeName} threw exception of type {exType}
                Message: {exMessage} 
                See stack trace: {trace}
                """
                , senderTypeName, exType, exMessage, trace);
            return;
        }
        Logger.Error("""
            {senderTypeName} threw exception of type {exType}
            Message: {exMessage} 
            See stack trace: {trace} 
            Here is also additional info: {additionalInfo}"
            """,
            senderTypeName, exType, exMessage, trace, additionalInfo);
    }


    private bool LogFileExist()
    {
        try
        {
            string dirName = Path.GetDirectoryName(LogFilePath) ?? string.Empty;
            if (Directory.Exists(dirName) is false)
            {
                Directory.CreateDirectory(dirName);
            }
            if (Path.Exists(LogFilePath) is false)
            {
                File.Create(LogFilePath);
            }
            return Path.Exists(LogFilePath);
        }
        catch
        {
#if DEBUG
            throw;
#else
            return false;
#endif
        }
    }
}
