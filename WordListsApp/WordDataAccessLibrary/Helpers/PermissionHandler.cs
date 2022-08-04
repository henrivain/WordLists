using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace WordDataAccessLibrary.Helpers;
internal class PermissionHandler
{
    internal static async Task<bool> RequestFileSystemWriteAccess()
    {
        PermissionStatus status = PermissionStatus.Granted;
        if (await CheckStatusAsync<StorageWrite>() is not PermissionStatus.Granted)
        {
            Debug.WriteLine("Request permission to use StorageWrite");
            status = await RequestAsync<StorageWrite>();
        }
        Debug.WriteLine($"Filesystem wrote access is set to: {status}");
        return status is PermissionStatus.Granted;
    }
    internal static async Task<bool> RequestFileSystemReadAccess()
    {
        PermissionStatus status = PermissionStatus.Granted;
        if (await CheckStatusAsync<StorageRead>() is not PermissionStatus.Granted)
        {
            Debug.WriteLine("Request permission to use StorageRead");
            status = await RequestAsync<StorageRead>();
        }
        Debug.WriteLine($"Filesystem read access is set to: {status}");
        return status is PermissionStatus.Granted;
    }
}
