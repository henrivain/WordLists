using WordDataAccessLibrary.Helpers;
using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordDataPages.JsonImportPage;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsUI.WordTrainingPages.StartTrainingPage;

namespace WordListsUI.HomePage;

public partial class HomePage : ContentPage
{
    ILogger<HomePage> Logger { get; }

    public HomePage(ILogger<HomePage> logger)
    {
        InitializeComponent();
        Logger = logger;

        // This line under is for development (automatically moving to the right page) and can be removed safely
        // _ = Shell.Current.GoToAsync($"{PageRoutes.Get(Route.Training)}/{nameof(TrainingConfigPage)}");
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

    private void CreateNewOCrField_Tapped(object sender, EventArgs e)
    {

    }
}
