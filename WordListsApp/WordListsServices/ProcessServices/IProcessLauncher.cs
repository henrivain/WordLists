namespace WordListsServices.ProcessServices;
public interface IProcessLauncher
{
    /// <summary>
    /// Get directory from given path and open the directory in file explorer.
    /// </summary>
    /// <param name="path">Path to file that is contained by the directory or path to directory ending in Path.(Alt)DirectorySeparatorChar.</param>
    /// <returns>IActionResult with success bool and status message representing action status.</returns>
    IActionResult LaunchFileExplorerToDirectory(string? path);
}
