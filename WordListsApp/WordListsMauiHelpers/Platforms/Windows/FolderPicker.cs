using Windows.Storage;
using WinRT.Interop;
using WordListsMauiHelpers.DeviceAccess;
using WindowsFolderPicker = Windows.Storage.Pickers.FolderPicker;

#nullable disable

namespace WordListsMauiHelpers;
public class FolderPicker : IFolderPicker
{
    public async Task<string> PickAsync()
    {
        StorageFolder result = null;
        try
        {
            WindowsFolderPicker picker = new();

            var windowHandle = ((MauiWinUIWindow)Application.Current.Windows[0].Handler.PlatformView).WindowHandle;

            InitializeWithWindow.Initialize(picker, windowHandle);

            result = await picker.PickSingleFolderAsync();
        }
        catch
        {
#if DEBUG
            throw;
#endif
        }
        if (result?.Path is null)
        {
            return null;
        }
        return result.Path;

    }
}
