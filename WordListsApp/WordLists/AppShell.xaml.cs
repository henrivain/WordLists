using WordListsMauiHelpers.PageRouting;
using WordListsUI.AppInfoPage;
using WordListsUI.HomePage;
using WordListsUI.WordDataPages.JsonImportPage;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsUI.StartTrainingPage;
using WordListsUI.WordDataPages.WordCollectionEditPage;
using WordListsUI.WordTrainingPage;
using WordListsUI.WordDataPages;
using WordListsUI.WordDataPages.JsonExportPage;

namespace WordLists;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        string training = PageRoutes.GetParentRoute(Route.Training);
        string handling = PageRoutes.GetParentRoute(Route.WordHandling);
        string lifetime = PageRoutes.GetParentRoute(Route.LifeTime);
        string backup = PageRoutes.GetParentRoute(Route.Backup);

        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(AppInfoPage), typeof(AppInfoPage));

        Routing.RegisterRoute($"{training}/{nameof(StartTrainingPage)}", typeof(StartTrainingPage));
		Routing.RegisterRoute($"{training}/{nameof(WordTrainingPage)}", typeof(WordTrainingPage));

        Routing.RegisterRoute($"{handling}/{nameof(WordDataPage)}", typeof(WordDataPage));
		Routing.RegisterRoute($"{handling}/{lifetime}/{nameof(ListGeneratorPage)}", typeof(ListGeneratorPage));
        Routing.RegisterRoute($"{handling}/{lifetime}/{nameof(WordCollectionEditPage)}", typeof(WordCollectionEditPage));

        Routing.RegisterRoute($"{handling}/{backup}/{nameof(JsonExportPage)}", typeof(JsonExportPage));
		Routing.RegisterRoute($"{handling}/{backup}/{nameof(JsonImportPage)}", typeof(JsonImportPage));

    }
}
