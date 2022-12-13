using WordListsViewModels.Events;

namespace WordListsViewModels.Interfaces;
public interface IAppInfoViewModel
{
    string AppVersion { get; }

    string AppEnvironment { get; }

    string DotNetVersion { get; }

    IAsyncRelayCommand PullLogsToDownloads { get; }

    event LogsCopiedEventHandler? LogsCopied;
}
