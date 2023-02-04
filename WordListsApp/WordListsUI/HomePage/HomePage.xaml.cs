using WordDataAccessLibrary.Helpers;
using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordDataPages.JsonImportPage;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsUI.WordTrainingPages.StartTrainingPage;
using WordListsUI.WordTrainingPages.TrainingConfigPage;

namespace WordListsUI.HomePage;

public partial class HomePage : ContentPage
{
    ILogger<HomePage> Logger { get; }

    public HomePage(ILogger<HomePage> logger)
    {
        InitializeComponent();
        Logger = logger;
        _ = CheckCommandLineArgs();

        // This line under is for development (automatically moving to the right page) and can be removed safely
        // _ = Shell.Current.GoToAsync($"{PageRoutes.Get(Route.Training)}/{nameof(TrainingConfigPage)}");
    }

    static bool CommandLineArgsChecked { get; set; } = false;




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
    private async Task CheckCommandLineArgs()
    {
        if (CommandLineArgsChecked)
        {
            return;
        }
        CommandLineArgsChecked = true;
        if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
        {
            return;
        }
        try
        {
            string appFileExtension = AppFileExtension.Get(FileExtension.Wordlist);
            var path = Environment.GetCommandLineArgs()
                .Where(x => x is not null)
                .Where(x => Path.GetExtension(x) == appFileExtension && Path.Exists(x))
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(path))
            {
                Logger.LogInformation("No valid {extension} paths in command line args.", appFileExtension);
                return;
            }
            Logger.LogInformation("Path '{path}' is a path to .wordlist file", path);
            await Shell.Current.GoToAsync($"{PageRoutes.Get(Route.WordHandling)}/{PageRoutes.Get(Route.Backup)}/{nameof(JsonImportPage)}", new Dictionary<string, object>()
            {
                [nameof(JsonImportPage.DefaultImportPath)] = path
            });
        }
        catch (NotSupportedException)
        {
            Logger.LogError("Cannot get command line args in current platform.");
            return;
        }

    }
}
