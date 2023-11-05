namespace WordListsMauiHelpers.Logging;
internal static class LogFileHelper
{
    const long OneGB = 1L * 1024 * 1024 * 1024;

    /// <summary>
    /// Check if file size is more than given max value. 
    /// If file size is bigger than maxSizeBytes the file is deleted.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="maxSizeBytes"></param>
    /// <param name="linesToCopy"></param>
    /// <returns>error message string if failed, otherwise null</returns>
    internal static string? ClearIfOverLimit(string path, long? maxSizeBytes = OneGB, int linesToCopy = 50)
    {
        maxSizeBytes ??= OneGB;
        if (File.Exists(path) is false)
        {
            return null;
        }
        try
        {
            if (new FileInfo(path).Length < maxSizeBytes)
            {
                return null;
            }
            return RemoveFileText(path, linesToCopy, true);
        }
        catch (Exception ex)
        {
            return $"""
                Failed to check weather log file size exceeded limit of '{maxSizeBytes}' bytes or not.
                Reason for error was '{ex.GetType().Name}': {ex.Message}"
                """;
        }
    }
    internal static string? RemoveFileText(string path, int leaveLines = 0, bool leaveFromEnd = false)
    {
        try
        {
            int fileLineCount = File.ReadLines(path).Count();
            if (fileLineCount < leaveLines)
            {
                leaveLines = fileLineCount;
            }

            string[] linesToLeave;
            if (leaveFromEnd)
            {
                // lines from end
                linesToLeave = File.ReadLines(path).Skip(fileLineCount - leaveLines).Take(leaveLines).ToArray();
            }
            else
            {
                // lines from start
                linesToLeave = File.ReadLines(path).Take(leaveLines).ToArray();
            }
            using StreamWriter writer = File.CreateText(path);
            writer.Flush();
            if (linesToLeave.Length > 0)
            {
                writer.WriteLine($"Next '{linesToLeave.Length}' lines are coming from old file. " +
                    $"They are copied when old file reached certain file size.");
            }
            foreach (var line in linesToLeave)
            {
                writer.WriteLine(line);
            }

            return null;
        }
        catch (Exception ex)
        {
            return $"Failed to remove lines from file because of '{ex.GetType().Name}': '{ex.Message}'";
        }
    }
}
