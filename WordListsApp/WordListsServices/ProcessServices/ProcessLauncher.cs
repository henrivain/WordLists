using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using WordListsServices.FileSystemServices;

namespace WordListsServices.ProcessServices;
public class ProcessLauncher : SafeFileHandler, IProcessLauncher
{
    public ProcessLauncher(ILogger logger) : base(logger) { }

    [UnsupportedOSPlatform("ios")]
    public IActionResult LaunchFileExplorerToDirectory(string? path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) is false)
        {
            Logger.LogWarning("Cannot launch file explorer in non-windows devices.");
            return new ActionResult(false)
            {
                Message = "Cannot open file explorer on non-windows device."
            };
        }
        if (TryGetDirectory(path, out var directory) is false)
        {
            Logger.LogWarning("Cannot get directory from path '{path}'.", path);
            return new ActionResult(false)
            {
                Message = "Cannot read direcory from full path."
            };
        }
        if (Directory.Exists(directory) is false)
        {
            Logger.LogWarning("Directory at '{dir}' does not exist.", directory);
            return new ActionResult(false)
            {
                Message = "Target directory doesn't exist."
            };
        }

        Logger.LogInformation("Launch file explorer window at {dir}", directory);

        try
        {
            Process.Start(new ProcessStartInfo()
            {
                Arguments = directory,
                FileName = "explorer.exe"
            });
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Failed to launch new file explorer window, because of '{ex}': {msg}", 
                ex.GetType().Name, ex.Message);
            return new ActionResult(false)
            {
                Message = $"Failed to launch new file explorer window, because of '{ex.GetType()}': {ex.Message}"
            };
        }
        Logger.LogInformation("File explorer launched successfully.");
        return new ActionResult(true);
    }
}
