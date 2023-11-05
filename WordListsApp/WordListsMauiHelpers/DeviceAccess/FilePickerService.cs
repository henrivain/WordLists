using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using WordDataAccessLibrary.Helpers;

namespace WordListsMauiHelpers.DeviceAccess;
public class FilePickerService : IFilePickerService
{
    public FilePickerService(ILogger logger)
    {
        Logger = logger;
    }

    public ReadOnlyDictionary<FileExtension, string> AppFileExtensions { get; } = GetExtensions();

    public async Task<string?> PickFile(List<string> acceptableExtensions)
    {
        Logger.LogInformation("Let user select file location.");
        return await DeviceSpecificFilePicker.GetUserSelectedFullPath(acceptableExtensions);
    }


#if WINDOWS
    public async Task<string?> PickFolder()
    {
        Logger.LogInformation("Let user pick folder location from their device.");
        return await DeviceSpecificFilePicker.GetUserSelectedFolder();
    }
#else
    public Task<string?> PickFolder()
    {
        Logger.LogError("Cannot pick folder in platform other than Windows currently.");
        throw new PlatformNotSupportedException();
    }
#endif

    public string GetAppFileExtension()
    {
        return AppFileExtensions[FileExtension.Wordlist];
    }

    ILogger Logger { get; }
    private static ReadOnlyDictionary<FileExtension, string> GetExtensions()
    {
        var dictionary = new Dictionary<FileExtension, string>()
        {
            [FileExtension.Json] = ".json",
            [FileExtension.Zip] = ".zip",
            [FileExtension.Wordlist] = ".wordlist",
        };
        return new(dictionary);
        
    }
}
