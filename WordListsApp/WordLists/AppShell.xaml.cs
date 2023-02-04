using WordListsMauiHelpers.PageRouting;


namespace WordLists;

public partial class AppShell : Shell
{
    public AppShell()
	{
		InitializeComponent();

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

    private void TabBar_Appearing(object sender, EventArgs e)
    {
        if (sender is TabBar bar)
        {
        }
    }
}
