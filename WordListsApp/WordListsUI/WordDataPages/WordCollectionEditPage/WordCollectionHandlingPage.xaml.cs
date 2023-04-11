using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsViewModels.Events;

namespace WordListsUI.WordDataPages.WordCollectionEditPage;

public partial class WordCollectionEditPage : ContentPage
{
	public WordCollectionEditPage(IWordCollectionHandlingViewModel model, ILogger<ContentPage> logger)
	{
		BindingContext = model;
		InitializeComponent();
        Model.CollectionsDeleted += Model_CollectionDeleted;
		Model.DeleteRequested += Model_DeleteRequested;
        Model.EditRequested += Model_EditRequested;
        Logger = logger;
    }

    IWordCollectionHandlingViewModel Model => (IWordCollectionHandlingViewModel)BindingContext;
    ILogger<ContentPage> Logger { get; }

    private async void Model_EditRequested(object sender, WordCollection collection)
    {
        string path = $"{PageRoutes.Get(Route.WordHandling)}/{PageRoutes.Get(Route.LifeTime)}/{nameof(ListGeneratorPage.ListGeneratorPage)}";

        await Shell.Current.GoToAsync(path, new Dictionary<string, object>()
        {
            [nameof(ListGeneratorPage.ListGeneratorPage.PageModeParameter)] = new PageModeParameter<ListGeneratorMode>()
            {
                Mode = ListGeneratorMode.Edit,
                Data = collection
            }
        });
    }

    private async void Model_DeleteRequested(object sender, DeleteWantedEventArgs e)
	{
		if (e.ItemsToDelete is null)
		{
			Logger.LogWarning("{cls}: Cannot delete wordcollections, none where provided.", 
				nameof(WordCollectionEditPage));
			return;
		}
		bool proceed;
		if (e.DeletesAll)
		{
			proceed = await DisplayAlert("Poista kaikki sanastot?", 
				$"Haluatko varmasti poistaa kaikki '{e.ItemsToDelete.Length}' sanastoa lopullisesti.", "Jatka", "Peruuta");
		}
		else
		{
            string message = $"""
				Haluatko varmasti poistaa {e.ItemsToDelete.Length} sanastoa lopullisesti?
				Painamalla 'Jatka', seuraavat sanastot poistetaan: 
				{string.Join("\n", e.ItemsToDelete.Select(x => x.Name))}
				""";

			proceed = await DisplayAlert("Poista sanastoja", message, "Jatka", "Peruuta");
		}
		if (proceed)
		{
            await Model.DeleteCollections(e.ItemsToDelete);
        }
    }

    private async void Model_CollectionDeleted(object sender, DataBaseActionArgs e)
	{
		int amountDeleted = e.CollectionNames.Length;

        await DisplayAlert("Poistettu onnistuneesti!", $"{amountDeleted} sanastoa poistettiin onnistuneesti.", "OK");
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
	{
		await Model.ResetCollections();
	}

    private void HideMenu(object sender, EventArgs e) => menu.Collapse(sender, e);
}