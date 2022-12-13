namespace WordListsServices.Extensions;
internal static class FileSystemExtensions
{
    internal static bool NotSuccess(this IActionResult result) => result.Success is false;

    internal static string? AddDirSeparator(this string? dirPath)
    {
        if (dirPath is null)
        {
            return null;
        }
        if (dirPath.EndsWith(Path.DirectorySeparatorChar) ||
            dirPath.EndsWith(Path.AltDirectorySeparatorChar))
        {
            return dirPath;
        }
        return dirPath + Path.DirectorySeparatorChar;
    }

}
