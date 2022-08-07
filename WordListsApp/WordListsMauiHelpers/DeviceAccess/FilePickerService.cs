using System.Diagnostics;
using System.Runtime.CompilerServices;
using WordDataAccessLibrary.Helpers;

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
#if ANDROID
                FileTypes = GetFileTypesWithExtension(null)     // file extension don't work currently 7.8.2022
#else
                FileTypes = GetFileTypesWithExtension(fileExtensions)
#endif
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

    /// <summary>
    /// Get path to .json file. In windows, user choose folder and filename is automatically generated. 
    /// On other platforms user choose .json file and its path will be returned;
    /// </summary>
    /// <returns>path to json file or null, if fails or user exits</returns>
    public static async Task<string> GetUserSelectedExportPath()
    {
#if WINDOWS
        string resultPath = await new FolderPicker().PickAsync();
        if (string.IsNullOrWhiteSpace(resultPath)) return null;
        return Path.Combine(resultPath, PathHelper.GetNewBackupFileName());
#else
        return await GetUserSelectedFullPath(new()
        {
            AppFileExtension.Get(FileExtension.Wordlist)
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
        extensions ??= new();

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
