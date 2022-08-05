using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WordListsMauiHelpersTests")]

namespace WordListsMauiHelpers.DeviceAccess;
public static class FilePickerService
{

    /// <param name="fileExtensions"></param>
    /// <returns>full path to selected file or null if user exits or something fails</returns>
    public static async Task<string> GetUserSelectedFullPath(List<string> fileExtensions)
    {
        try
        {
            FileResult result = await FilePicker.Default.PickAsync(new()
            {
                PickerTitle = "Valitse sijainti vietävälle tiedostolle",
                FileTypes = GetFileTypesWithExtension(fileExtensions ?? new())
            });
            if (string.IsNullOrWhiteSpace(result?.FullPath)) return null;
            return result.FullPath;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Try pick file: exception: {ex.Message}");
            return null;
        }
    }


    public static async Task<string> GetUserSelectedJsonExportPath()
    {
#if WINDOWS
        string resultPath = await new FolderPicker().PickAsync();
        if (string.IsNullOrWhiteSpace(resultPath)) return null;
        return Path.Combine(resultPath, PathHelper.GetNewExportFileName());
#else
        return await GetUserSelectedFullPath(new()
        {
            ".json"
        });
#endif
    }

    
    internal static List<string> GetValidFileExtensions(List<string> extensions)
    {
        extensions ??= new();
        return extensions.Where(x => string.IsNullOrWhiteSpace(x) is false)
                         .Select(x => x.StartsWith(".") ? x : $".{x}")
                         .ToList();
    }

    private static FilePickerFileType GetFileTypesWithExtension(List<string> extensions)
    {


        return new(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, extensions },
                    { DevicePlatform.Android, extensions },
                    { DevicePlatform.WinUI, extensions },
                    { DevicePlatform.Tizen, extensions },
                    { DevicePlatform.macOS, extensions }
                });
    }

}
