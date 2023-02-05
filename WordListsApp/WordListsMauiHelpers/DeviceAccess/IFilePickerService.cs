using System.Collections.ObjectModel;
using WordDataAccessLibrary.Helpers;

namespace WordListsMauiHelpers.DeviceAccess;
public interface IFilePickerService
{
    /// <summary>
    /// Pick folder with given file extension
    /// </summary>
    /// <param name="acceptableExtensions"></param>
    /// <returns>path string if user chose one, otherwise null</returns>
    Task<string?> PickFile(List<string> acceptableExtensions);

    /// <summary>
    /// Pick folder from local device
    /// <para/>!!! NOTE: this functionality might not work in every platform => see default implementation from injections !!!
    /// </summary>
    /// <returns></returns>
    Task<string?> PickFolder();

    /// <summary>
    /// Get file extension that the application defines (.wordlist)
    /// </summary>
    string GetAppFileExtension();

    /// <summary>
    /// Get all file extensions application is using
    /// </summary>
    ReadOnlyDictionary<FileExtension, string> AppFileExtensions { get; }
}
