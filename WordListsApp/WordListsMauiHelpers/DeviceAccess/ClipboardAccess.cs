namespace WordListsMauiHelpers.DeviceAccess;
public static class ClipboardAccess
{
    /// <summary>
    /// Get string from Clipboard asynchronously
    /// </summary>
    /// <returns>string from clipboard or empty if null</returns>
    public static async Task<string> GetStringAsync()
    {
        return await Clipboard.Default.GetTextAsync() ?? string.Empty;
    }

    /// <summary>
    /// Set string to clipboard async
    /// </summary>
    /// <param name="text"></param>
    /// <returns>awaitable task</returns>
    public static async Task SetStringAsync(string text)
    {
        if (text is null) return;
        await Clipboard.SetTextAsync(text);
    }
}
