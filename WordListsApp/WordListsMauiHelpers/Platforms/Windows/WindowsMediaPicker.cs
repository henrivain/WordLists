using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using WinRT.Interop;
using Launcher = Windows.System.Launcher;
using Window = Microsoft.Maui.Controls.Window;

namespace WordListsMauiHelpers.Platforms.Windows;


public class WindowsMediaPicker : IMediaPicker
{
    public WindowsMediaPicker(ILogger? logger = null, Window? host = null)
    {
        Logger = logger ?? NullLogger.Instance;
        TryLoadNativeCapture(host);
    }

    public bool IsCaptureSupported { get; } = true;
    
    
    LauncherOptions? LauncherOptions { get; set; }
    bool IsNativeCaptureSupported { get; set; }
    ILogger Logger { get; }

    public async Task<FileResult> CapturePhotoAsync(MediaPickerOptions? options = null)
    {
        // Check if native capture can be downloaded 
        if (TryLoadNativeCapture() is false)
        {
            Logger.LogInformation("Cannot use native photo capture in {cls}, use Maui implementation.", 
                nameof(WindowsMediaPicker));
            return await MediaPicker.Default.CapturePhotoAsync(options);
        }

#nullable disable
        if (IsCaptureSupported is false || LauncherOptions is null)
        {
            Logger.LogError("Cannot capture photo, because device camera was not initialized successfully.");
            return null;
        }

        var tempFolder = ApplicationData.Current.TemporaryFolder;
        var baseFileName = "CCapture.jpg";
        var tempFile = await tempFolder.CreateFileAsync(baseFileName, CreationCollisionOption.GenerateUniqueName);
        var token = SharedStorageAccessManager.AddFile(tempFile);

        var set = new ValueSet()
        {
            { "MediaType", "photo" },
            { "PhotoFileToken", token }
        };

        var uri = new Uri("microsoft.windows.camera.picker:");
        var result = await Launcher.LaunchUriForResultsAsync(uri, LauncherOptions, set);
        if (result.Status is not LaunchUriStatus.Success)
        {
            return null;
        }
#nullable enable
        var type = tempFile.ContentType;
        var path = tempFile.Path;

        Logger.LogInformation("Captured image '{path}'", path);
        return new(path, type);
    }


    public async Task<FileResult> PickPhotoAsync(MediaPickerOptions? options = null)
    {
        return await MediaPicker.Default.PickPhotoAsync(options);
    }



    public async Task<FileResult> PickVideoAsync(MediaPickerOptions? options = null)
    {
        return await MediaPicker.Default.PickVideoAsync(options);
    }

    public async Task<FileResult> CaptureVideoAsync(MediaPickerOptions? options = null)
    {
        return await MediaPicker.Default.CaptureVideoAsync(options);
    }


    private bool TryLoadNativeCapture(Window? host = null)
    {
        if (IsNativeCaptureSupported)
        {
            return true;
        }

        if (host is null && Application.Current?.Windows.Count <= 0)
        {
            IsNativeCaptureSupported = false;
            Logger.LogWarning("Cannot initialize native {cls}. " +
                "Main window is probably loaded yet and you haven't provided a window.",
                nameof(WindowsMediaPicker));
            return false;
        }
        host ??= Application.Current?.Windows[0];
        if (host is null)
        {
            IsNativeCaptureSupported = false;
            return false;
        }

        IntPtr handle = IntPtr.Zero;
        if (host.Handler.PlatformView is MauiWinUIWindow window)
        {
            handle = window.WindowHandle;
        }
        if (handle == IntPtr.Zero)
        {
            IsNativeCaptureSupported = false;
            return false;
        }

        LauncherOptions = new();
        InitializeWithWindow.Initialize(LauncherOptions, handle);
        LauncherOptions.TreatAsUntrusted = false;
        LauncherOptions.DisplayApplicationPicker = false;
        LauncherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsCamera_8wekyb3d8bbwe";
        Logger.LogInformation("Initialized {cls} successfully.", nameof(WindowsMediaPicker));
        return true;
    }

}
