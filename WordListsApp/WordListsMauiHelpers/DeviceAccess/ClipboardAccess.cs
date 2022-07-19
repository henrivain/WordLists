using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDataAccessLibrary.DeviceAccess;
public static class ClipboardAccess
{
    /// <summary>
    /// Get string from Clipboard asynchronously
    /// </summary>
    /// <returns>string from clipboard or empty if null</returns>
    public static async Task<string> GetStringAsync()
    {
        return await Clipboard.GetTextAsync() ?? string.Empty;
    }

    /// <summary>
    /// Set string to clipboard async
    /// </summary>
    /// <param name="text"></param>
    /// <returns>awaitable task</returns>
    public static async Task SetStringAsync(string text)
    {
        await Clipboard.SetTextAsync(text);
    }
}
