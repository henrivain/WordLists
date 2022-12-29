using WordListsMauiHelpers.PageRouting;
using WordListsUI.AppInfoPage;
using WordListsUI.HomePage;
using WordListsUI.WordDataPages;
using WordListsUI.WordDataPages.ImageRecognisionPage;
using WordListsUI.WordDataPages.JsonExportPage;
using WordListsUI.WordDataPages.JsonImportPage;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsUI.WordDataPages.WordCollectionEditPage;
using WordListsUI.WordTrainingPages.FlipCardTrainingPage;
using WordListsUI.WordTrainingPages.StartTrainingPage;
using WordListsUI.WordTrainingPages.WritingTestPage;

namespace WordLists;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        string training = PageRoutes.GetRoute(Route.Training);
        string handling = PageRoutes.GetRoute(Route.WordHandling);
        string lifetime = PageRoutes.GetRoute(Route.LifeTime);
        string backup = PageRoutes.GetRoute(Route.Backup);

        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(AppInfoPage), typeof(AppInfoPage));

        Routing.RegisterRoute($"{training}/{nameof(StartTrainingPage)}", typeof(StartTrainingPage));
		Routing.RegisterRoute($"{training}/{nameof(FlipCardTrainingPage)}", typeof(FlipCardTrainingPage));
		Routing.RegisterRoute($"{training}/{nameof(WritingTestPage)}", typeof(WritingTestPage));
		Routing.RegisterRoute($"{training}/{nameof(WritingTestConfigurationPage)}", typeof(WritingTestConfigurationPage));
		Routing.RegisterRoute($"{training}/{nameof(WriteTestResultPage)}", typeof(WriteTestResultPage));

        Routing.RegisterRoute($"{handling}/{nameof(WordDataPage)}", typeof(WordDataPage));
		Routing.RegisterRoute($"{handling}/{lifetime}/{nameof(ListGeneratorPage)}", typeof(ListGeneratorPage));
		Routing.RegisterRoute($"{handling}/{lifetime}/{nameof(ImageRecognisionPage)}", typeof(ImageRecognisionPage));
        Routing.RegisterRoute($"{handling}/{lifetime}/{nameof(WordCollectionEditPage)}", typeof(WordCollectionEditPage));
        Routing.RegisterRoute($"{handling}/{backup}/{nameof(JsonExportPage)}", typeof(JsonExportPage));
		Routing.RegisterRoute($"{handling}/{backup}/{nameof(JsonImportPage)}", typeof(JsonImportPage));
    }
}
