using WordListsMauiHelpers;
using WordListsMauiHelpers.Logging;
using WordListsServices.FileSystemServices;
using WordListsServices.FileSystemServices.ActionResults;
using WordListsViewModels.Events;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class AppInfoViewModel : IAppInfoViewModel
{
    public AppInfoViewModel(
        ILogger<AppInfoViewModel> logger, 
        ILoggingInfoProvider loggingInfoProvider,
        IFileHandler fileHandler)
    {
        Logger = logger;
        LoggingInfoProvider = loggingInfoProvider;
        FileHandler = fileHandler;
    }

    [ObservableProperty]
    string _appVersion = WordDataAccessLibrary.Helpers.AssemblyHelper.EntryAssembly.VersionString ?? "Ei saatavilla";

    [ObservableProperty]
    string _appEnvironment = GetAppEnvironment();

    [ObservableProperty]
    string _dotNetVersion = GetDotnetVersion();

    [ObservableProperty]
    bool _working = false;

    public IAsyncRelayCommand PullLogsToDownloads => new AsyncRelayCommand(CopyLogsToDownloads);

    public ILogger<AppInfoViewModel> Logger { get; }
    public ILoggingInfoProvider LoggingInfoProvider { get; }
    public IFileHandler FileHandler { get; }

    private static string GetDotnetVersion()
    {
        return Environment.Version.Major.ToString() + " MAUI";
    }

    private static string GetAppEnvironment()
    {
        return DeviceInfo.Current.Platform.ToString();
    }

    private async Task CopyLogsToDownloads()
    {
        Logger.LogInformation("User requested to copy log files to downloads folder.");

        Working = true;
        string[] logFiles = LoggingInfoProvider.LoggingFilePaths;
        string outputDir = Path.Combine(PathHelper.GetDownloadsFolderPath(), "WordLists_Logs\\");

        int filesValid = 0;
        int filesFailed = 0;
        IFileSystemResult? result = null;
        foreach (var file in logFiles)
        {
            result = await FileHandler.CopyFileAsync(file, outputDir);
            if (result.Success)
            {
                filesValid++;
                continue;
            }
            Logger.LogError("Failed to copy log file to downloads: {msg}", result.Message);
            filesFailed++;
        }
        Working = false;
        string msg;
        if (filesFailed > 0 && result is not null)
        {
            msg = $"""
                Failed to copy '{filesFailed}' files to downloads folder. 
                '{filesValid}' copies succeeded. 
                Here is error message from one (1) failed copy: 
                '{result}'
                """;
            LogsCopied?.Invoke(this, new LogsCopiedEventArgs
            {
                Success = false,
                FilesFailed = filesFailed,
                FilesValid = filesValid,
                Message =  msg,
                OutputDirectory = outputDir,
            });
        }
        if (filesFailed > 0)
        {
            msg = """
                Failed to copy '{filesFailed}' files to downloads folder. 
                '{filesValid}' copies succeeded.
                """;
            LogsCopied?.Invoke(this, new LogsCopiedEventArgs
            {
                Success = false,
                FilesFailed = filesFailed,
                FilesValid = filesValid,
                Message = msg,

                OutputDirectory = outputDir,
            });
            return; 
        }
        msg = $"Successfully copied '{filesValid}' log files to downloads folder.";
        LogsCopied?.Invoke(this, new LogsCopiedEventArgs
        { 
            Success = true,
            FilesFailed = 0,
            FilesValid = filesValid,
            Message = msg,
            OutputDirectory = outputDir,
        });

    }


    public event LogsCopiedEventHandler? LogsCopied;
}
