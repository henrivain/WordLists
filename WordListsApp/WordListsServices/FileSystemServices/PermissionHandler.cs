using System.Diagnostics;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace WordListsServices.FileSystemServices;
internal class PermissionHandler
{
    private ILogger Logger { get; }

    public PermissionHandler(ILogger logger)
    {
        Logger = logger;
    }

    internal async Task<bool> RequestFileSystemWriteAccess()
    {
        PermissionStatus status = PermissionStatus.Granted;
        if (await CheckStatusAsync<StorageWrite>() is not PermissionStatus.Granted)
        {
            Logger.LogInformation("Request permission to use StorageWrite");
            status = await RequestAsync<StorageWrite>();
        }
        Logger.LogInformation($"Filesystem wrote access is set to: {status}");
        return status is PermissionStatus.Granted;
    }
    internal async Task<bool> RequestFileSystemReadAccess()
    {
        PermissionStatus status = PermissionStatus.Granted;
        if (await CheckStatusAsync<StorageRead>() is not PermissionStatus.Granted)
        {
            Logger.LogInformation("Request permission to use StorageRead");
            status = await RequestAsync<StorageRead>();
        }
        Logger.LogInformation($"Filesystem read access is set to: {status}");
        return status is PermissionStatus.Granted;
    }
}
