using WordListsMauiHelpers;
using WordListsMauiHelpers.Logging;
using WordListsServices;
using WordListsServices.FileSystemServices;
using WordListsServices.FileSystemServices.ActionResults;
using WordListsServices.ProcessServices;
using WordListsViewModels.Events;

namespace WordListsViewModels;

[INotifyPropertyChanged]
public partial class AppInfoViewModel : IAppInfoViewModel
{
    public AppInfoViewModel(
        ILogger<AppInfoViewModel> logger,
        ILoggingInfoProvider loggingInfoProvider,
        IFileHandler fileHandler,
        IProcessLauncher processLauncher
        )
    {
        Logger = logger;
        LoggingInfoProvider = loggingInfoProvider;
        FileHandler = fileHandler;
        ProcessLauncher = processLauncher;
    }

    [ObservableProperty]
    string _appVersion = VersionTracking.CurrentVersion ?? "Ei saatavilla"; // AssemblyHelper.EntryAssembly.VersionString ?? "Ei saatavilla";

    [ObservableProperty]
    string _appEnvironment = GetAppEnvironment();

    [ObservableProperty]
    string _dotNetVersion = GetDotnetVersion();

    [ObservableProperty]
    bool _working = false;

    [ObservableProperty]
    bool _isDebug = GetIsDebug();


    public IAsyncRelayCommand PullLogsToDownloads => new AsyncRelayCommand(CopyLogsToDownloads);

    public ILogger<AppInfoViewModel> Logger { get; }
    public ILoggingInfoProvider LoggingInfoProvider { get; }
    public IFileHandler FileHandler { get; }
    public IProcessLauncher ProcessLauncher { get; }

    public IRelayCommand OpenLogsInFileExplorer => new RelayCommand(OpenLogDirectoryInFileExplorer);

    private static string GetDotnetVersion()
    {
        return Environment.Version.Major.ToString() + " MAUI";
    }
    private static string GetAppEnvironment()
    {
        return DeviceInfo.Current.Platform.ToString();
    }

    private static bool GetIsDebug()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }


    private async Task CopyLogsToDownloads()
    {
        Logger.LogInformation("User requested to copy log files to downloads folder.");

        Working = true;
        string[] logFiles = LoggingInfoProvider.LoggingFilePaths;
 
        string outputDir = Path.Combine(PathHelper.GetDownloadsFolderPath(), "WordLists_Logs" + Path.DirectorySeparatorChar);

        int filesValid = 0;
        int filesFailed = 0;
        IFileSystemResult? result = null;
        foreach (var file in logFiles)
        {
            result = await CopyFile(file, outputDir);

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
            msg = $"Failed to copy {filesFailed}/{logFiles.Length} files to downloads folder. " +
                $"One of the error messages: '{result.Message}'";
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
        if (filesFailed > 0)
        {
            msg = $"Failed to copy {filesFailed}/{logFiles.Length} files to downloads folder.";
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

    private async Task<IFileSystemResult> CopyFile(string? file, string outputDir)
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            string? fileName = Path.GetFileName(file);
            return await FileHandler.CopyFileAsync(file, outputDir, $"{fileName}.txt");
        }
        return await FileHandler.CopyFileAsync(file, outputDir);
    }

    private void OpenLogDirectoryInFileExplorer()
    {
        var paths = LoggingInfoProvider.LoggingFilePaths;
        if (paths.Length <= 0)
        {
            Logger.LogInformation("Cannot open log file directory, because app does not have any.");
            ShowLogFileFailed?.Invoke(this, new InvalidDataEventArgs<string[]>
            {
                Message = "App currently does not have any log files."
            });
            return;
        }

        IActionResult? result = null;
        foreach (var path in paths)
        {
            result = ProcessLauncher.LaunchFileExplorerToDirectory(path);
            if (result.NotSuccess())
            {
                Logger.LogError("Cannot open file explorer window, because of '{reason}'.", result.Message);
            }
            else return;
        }
        ShowLogFileFailed?.Invoke(this, new(result?.Message, paths));
    }




    public event LogsCopiedEventHandler? LogsCopied;

    public event InvalidDataEventHandler<string[]>? ShowLogFileFailed;
}
