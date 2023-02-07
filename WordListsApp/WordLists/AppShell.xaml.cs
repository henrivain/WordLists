using Microsoft.Extensions.Logging;
using WordDataAccessLibrary.Helpers;
using WordListsMauiHelpers.PageRouting;


namespace WordLists;

public partial class AppShell : Shell
{
    public AppShell(ILogger logger)
	{
		InitializeComponent();
        Logger = logger;

        string training = PageRoutes.Get(Route.Training);
        string handling = PageRoutes.Get(Route.WordHandling);
        string lifetime = PageRoutes.Get(Route.LifeTime);
        string backup = PageRoutes.Get(Route.Backup);

        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(AppInfoPage), typeof(AppInfoPage));

        Routing.RegisterRoute($"{training}/{nameof(StartTrainingPage)}", typeof(StartTrainingPage));
		Routing.RegisterRoute($"{training}/{nameof(FlipCardTrainingPage)}", typeof(FlipCardTrainingPage));
		Routing.RegisterRoute($"{training}/{nameof(WritingTestPage)}", typeof(WritingTestPage));
		Routing.RegisterRoute($"{training}/{nameof(WritingTestConfigurationPage)}", typeof(WritingTestConfigurationPage));
		Routing.RegisterRoute($"{training}/{nameof(WriteTestResultPage)}", typeof(WriteTestResultPage));
		Routing.RegisterRoute($"{training}/{nameof(WordListPage)}", typeof(WordListPage));
		Routing.RegisterRoute($"{training}/{nameof(TrainingConfigPage)}", typeof(TrainingConfigPage));

        Routing.RegisterRoute($"{handling}/{nameof(WordDataPage)}", typeof(WordDataPage));
		Routing.RegisterRoute($"{handling}/{lifetime}/{nameof(ListGeneratorPage)}", typeof(ListGeneratorPage));
		Routing.RegisterRoute($"{handling}/{lifetime}/{nameof(ImageRecognisionPage)}", typeof(ImageRecognisionPage));
        Routing.RegisterRoute($"{handling}/{lifetime}/{nameof(WordCollectionEditPage)}", typeof(WordCollectionEditPage));
        Routing.RegisterRoute($"{handling}/{backup}/{nameof(JsonExportPage)}", typeof(JsonExportPage));
		Routing.RegisterRoute($"{handling}/{backup}/{nameof(JsonImportPage)}", typeof(JsonImportPage));
    }

    ILogger Logger { get; }
    bool CommandLineArgsChecked { get; set; } = false;

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
        Logger.LogInformation("Check if commandline args were provided.");
        try
        {
            string appFileExtension = AppFileExtension.Get(FileExtension.Wordlist);
            var path = Environment.GetCommandLineArgs()
                .Where(x => x is not null)
                .Where(x => Path.GetExtension(x) == appFileExtension && Path.Exists(x))
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(path))
            {
                Logger.LogInformation("No path with app file extension {extension} was provided, no need to open edit page.", appFileExtension);
                return;
            }
            Logger.LogInformation("Path '{path}' is a path to .wordlist file", path);
            await Current.GoToAsync($"{PageRoutes.Get(Route.WordHandling)}/{PageRoutes.Get(Route.Backup)}/{nameof(JsonImportPage)}", new Dictionary<string, object>()
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

  
    private async void Shell_Loaded(object sender, EventArgs e)
    {
        await CheckCommandLineArgs();
    }
}
