using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordListsMauiHelpers.DeviceAccess;
public class FilePickerService
{

    /// <param name="fileExtensions"></param>
    /// <returns>full path to selected file or null if user exits or something fails</returns>
    public static async Task<string> GetUserSelectedFullPath(string fileExtensions)
    {
        PickOptions options = new()
        {
            PickerTitle = "Valitse sijainti vietävälle tieodstolle",
            FileTypes = GetFileTypesWithExtension(fileExtensions ?? string.Empty)
        };

        try
        {
            FileResult result = await FilePicker.Default.PickAsync(options);
            if (string.IsNullOrWhiteSpace(result?.FullPath)) return null;
            return result.FullPath;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Try pick file: exception: {ex.Message}");
            return null;
        }
    }

    private static FilePickerFileType GetFileTypesWithExtension(string extension)
    {
        if (string.IsNullOrEmpty(extension))
        {
            throw new ArgumentNullException(nameof(extension));
        }
        if (extension.StartsWith(".") is false)
        {
            extension = $".{extension}";
        }

        return new(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { extension } },
                    { DevicePlatform.Android, new[] { extension } },
                    { DevicePlatform.WinUI, new[] { extension } },
                    { DevicePlatform.Tizen, new[] { extension } },
                    { DevicePlatform.macOS, new[] { extension } }
                });
    }

}
