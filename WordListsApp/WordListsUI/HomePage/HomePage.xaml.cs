#if WINDOWS
using Microsoft.UI.Xaml;
using Windows.System;
#endif
using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordDataPages.JsonImportPage;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsUI.WordDataPages.OcrListGeneratorPage;
using WordListsUI.WordTrainingPages.StartTrainingPage;

namespace WordListsUI.HomePage;

public partial class HomePage : ContentPage
{
    ILogger<HomePage> Logger { get; }

    public HomePage(ILogger<HomePage> logger)
    {
        InitializeComponent();
        Logger = logger;
        Focus();
    }


    private async void ImportField_Tapped(object sender, EventArgs e)
    {
        Logger.LogInformation("Goto '{destination}' from '{this}'", nameof(JsonImportPage), nameof(HomePage));
        await Shell.Current.GoToAsync($"{PageRoutes.Get(Route.WordHandling)}/{PageRoutes.Get(Route.Backup)}/{nameof(JsonImportPage)}");
    }
    private async void CreateNewField_Tapped(object sender, EventArgs e)
    {
        Logger.LogInformation("Goto '{destination}' from '{this}'", nameof(ListGeneratorPage), nameof(HomePage));
        await Shell.Current.GoToAsync($"{PageRoutes.Get(Route.WordHandling)}/{PageRoutes.Get(Route.LifeTime)}/{nameof(ListGeneratorPage)}");
    }
    private async void StartTrainingField_Tapped(object sender, EventArgs e)
    {
        Logger.LogInformation("Goto '{destination}' from '{this}'", nameof(StartTrainingPage), nameof(HomePage));
        await Shell.Current.GoToAsync($"{PageRoutes.Get(Route.Training)}/{nameof(StartTrainingPage)}");
    }

    private async void CreateNewOcrField_Tapped(object sender, EventArgs e)
    {
        Logger.LogInformation("Goto '{destination}' from '{this}'", nameof(BaseOcrListGeneratorPage), nameof(HomePage));
        await Shell.Current.GoToAsync($"{PageRoutes.Get(Route.WordHandling)}/{PageRoutes.Get(Route.LifeTime)}/{nameof(BaseOcrListGeneratorPage)}");
    }

 

#if WINDOWS
    // This does not work currently, because page cannot be focused (needs entry)
    protected override void OnHandlerChanged()
    {
        if (Handler?.PlatformView is UIElement element)
        {
            element.KeyDown += (sender, e) =>
            {
                switch (e.Key)
                {
                    case (VirtualKey.L):
                        StartTrainingField_Tapped(sender, EventArgs.Empty);
                        break;
                    case (VirtualKey.N):
                        CreateNewField_Tapped(sender, EventArgs.Empty);
                        break;
                    case (VirtualKey.I):
                        ImportField_Tapped(sender, EventArgs.Empty);
                        break;
                    case (VirtualKey.O):
                        CreateNewOcrField_Tapped(sender, EventArgs.Empty);
                        break;
                }
            };

        }

    }
#endif
}
