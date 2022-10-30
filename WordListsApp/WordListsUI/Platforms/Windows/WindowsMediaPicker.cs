using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.System;
using WinRT.Interop;
using Launcher = Windows.System.Launcher;
using Window = Microsoft.Maui.Controls.Window;

namespace WordListsUI.Platforms.Windows;


public class WindowsMediaPicker
{
    private readonly LauncherOptions _launcherOptions;

    public WindowsMediaPicker(Window window)
    {
        IntPtr handle = ((MauiWinUIWindow)window.Handler.PlatformView).WindowHandle;
        _launcherOptions = new LauncherOptions();
        InitializeWithWindow.Initialize(_launcherOptions, handle);
        _launcherOptions.TreatAsUntrusted = false;
        _launcherOptions.DisplayApplicationPicker = false;
        _launcherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsCamera_8wekyb3d8bbwe";
    }

    public async Task<StorageFile> CaptureFileAsync()
    {
        var currentAppData = ApplicationData.Current;
        var tempLocation = currentAppData.TemporaryFolder;
        var tempFileName = "CCapture.jpg";
        var tempFile = await tempLocation.CreateFileAsync(tempFileName, CreationCollisionOption.GenerateUniqueName);
        var token = SharedStorageAccessManager.AddFile(tempFile);

        var set = new ValueSet()
        {
            { "MediaType", "photo" },
            { "PhotoFileToken", token }
        };

        var uri = new Uri("microsoft.windows.camera.picker:");
        var result = await Launcher.LaunchUriForResultsAsync(uri, _launcherOptions, set);
        if (result.Status is LaunchUriStatus.Success) return tempFile;
        return null;
    }


    internal static async Task<(string FileType, string FullPath)> Run()
    {
        WindowsMediaPicker picker = new(Application.Current.Windows[0]);
        StorageFile file = await picker.CaptureFileAsync();
        if (file is null) return (null, null);
        return (file.FileType, file.Path);
    }
}
