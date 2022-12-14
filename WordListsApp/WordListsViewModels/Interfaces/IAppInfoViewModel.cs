using WordListsViewModels.Events;

namespace WordListsViewModels.Interfaces;
public interface IAppInfoViewModel
{
    string AppVersion { get; }

    string AppEnvironment { get; }

    string DotNetVersion { get; }

    bool Working { get; }

    IAsyncRelayCommand PullLogsToDownloads { get; }

    IRelayCommand OpenLogsInFileExplorer { get; }

    event LogsCopiedEventHandler? LogsCopied;

    event InvalidDataEventHandler<string[]>? ShowLogFileFailed;
}
