using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using WinRT.Interop;
using Launcher = Windows.System.Launcher;
using Window = Microsoft.Maui.Controls.Window;

namespace WordListsMauiHelpers;


public class WindowsMediaPicker : IMediaPicker
{
    public WindowsMediaPicker(ILogger? logger = null, Window? host = null)
    {
        Logger = logger ?? NullLogger.Instance;
        if (TryLoadNativeCapture(host) is false)
        {
            Logger.LogInformation("Native image capture is not supported.");
        }
        
    }

    public bool IsCaptureSupported => IsNativeCaptureSupported || MediaPicker.Default.IsCaptureSupported;
    
    
    LauncherOptions? LauncherOptions { get; set; }
    bool IsNativeCaptureSupported { get; set; }
    ILogger Logger { get; }

    public async Task<FileResult> CapturePhotoAsync(MediaPickerOptions? options = null)
    {
        Logger.LogInformation("Native capture state {}.", IsNativeCaptureSupported);
        Logger.LogInformation("Maui capture state {}.", MediaPicker.Default.IsCaptureSupported);
        
        // Check if native capture can be downloaded 
        if (TryLoadNativeCapture() is false)
        {
            Logger.LogInformation("Cannot use native photo capture in {cls}, use Maui implementation.", 
                nameof(WindowsMediaPicker));
            IsNativeCaptureSupported = false;
            return await MediaPicker.Default.CapturePhotoAsync(options);
        }
        Logger.LogInformation("Capture photo using native implementation.");

#nullable disable
        if (IsCaptureSupported is false || LauncherOptions is null)
        {
            Logger.LogError("Cannot capture photo, because device camera was not initialized successfully.");
            return null;
        }
#nullable enable

        string cacheDir = FileSystem.Current.CacheDirectory;
        string cameraFolder = Path.Combine(cacheDir, "camera");
        try
        {
            Directory.CreateDirectory(cameraFolder);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Cannot create camera folder, '{ex}': {msg}", ex.GetType().Name, ex.Message);
#nullable disable
            return null;
#nullable enable
        }
        
        StorageFolder tempFolder;
        try
        {
            tempFolder = await StorageFolder.GetFolderFromPathAsync(cameraFolder);
        }
        catch (FileNotFoundException ex)
        {
            Logger.LogError(ex, "Cannot get camera folder, '{ex}': {msg}", ex.GetType().Name, ex.Message);
#nullable disable
            return null;
#nullable enable
        }

        var baseFileName = "CCapture.jpg";
        StorageFile? tempFile = await tempFolder.CreateFileAsync(baseFileName, CreationCollisionOption.GenerateUniqueName);
        if (tempFile is null)
        {
#nullable disable
            Logger.LogInformation("Cannot create temporary file for camera capture.");
            return null;
#nullable enable
        }

        Logger.LogInformation("Created file '{file}' successfully.", tempFile.Path);

        var token = SharedStorageAccessManager.AddFile(tempFile);

        var set = new ValueSet()
        {
            { "MediaType", "photo" },
            { "PhotoFileToken", token }
        };

#nullable disable
        var uri = new Uri("microsoft.windows.camera.picker:");
        var result = await Launcher.LaunchUriForResultsAsync(uri, LauncherOptions, set);
        IsNativeCaptureSupported = true;
        if (result.Result is null)
        {
            return null;
        }
        if (result.Status is not LaunchUriStatus.Success)
        {
            Logger.LogWarning("Failed to launch camera app, status: '{status}'.", result.Status);
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
        try
        {
            InitializeWithWindow.Initialize(LauncherOptions, handle);
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Failed to initialize windows camera, '{ex}': '{msg}'", ex.GetType().Name, ex.Message);
            return false;
        }
        LauncherOptions.TreatAsUntrusted = false;
        LauncherOptions.DisplayApplicationPicker = false;
        LauncherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsCamera_8wekyb3d8bbwe";
        Logger.LogInformation("Initialized {cls} successfully.", nameof(WindowsMediaPicker));
        return true;
    }

}
