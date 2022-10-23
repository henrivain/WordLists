using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordDataPages.JsonImportPage;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsUI.WordTrainingPages.StartTrainingPage;

namespace WordListsUI.HomePage;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

	private async void ImportField_Tapped(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync($"{PageRoutes.GetRoute(Route.WordHandling)}/{PageRoutes.GetRoute(Route.Backup)}/{nameof(JsonImportPage)}");
	}

	private async void CreateNewField_Tapped(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync($"{PageRoutes.GetRoute(Route.WordHandling)}/{PageRoutes.GetRoute(Route.LifeTime)}/{nameof(ListGeneratorPage)}");
	}

	private async void StartTrainingField_Tapped(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync($"{PageRoutes.GetRoute(Route.Training)}/{nameof(StartTrainingPage)}");
    }
}