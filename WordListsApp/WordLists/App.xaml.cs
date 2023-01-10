
namespace WordLists;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		SetUIHandlers();
        MainPage = new AppShell();

		
	}
    private static void SetUIHandlers()
    {
#if ANDROID
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
            if (view is Entry)
            {
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        });
#endif
    }
}
