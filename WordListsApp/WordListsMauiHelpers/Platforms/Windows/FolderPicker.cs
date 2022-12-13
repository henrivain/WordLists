using WinRT.Interop;
using WindowsFolderPicker = Windows.Storage.Pickers.FolderPicker;
using Windows.Storage;

#nullable disable

namespace WordListsMauiHelpers.DeviceAccess;
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
