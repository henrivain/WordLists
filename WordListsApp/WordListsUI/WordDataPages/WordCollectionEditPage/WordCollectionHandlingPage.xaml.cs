namespace WordListsUI.WordDataPages.WordCollectionEditPage;

public partial class WordCollectionEditPage : ContentPage
{
	public WordCollectionEditPage(IWordCollectionHandlingViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
        Model.CollectionsDeleted += Model_CollectionDeleted;
		Model.DeleteRequested += Model_DeleteRequested;
    }

	private async void Model_DeleteRequested(object sender, WordListsViewModels.Events.DeleteWantedEventArgs e)
	{
		bool proceed;
		if (e.DeletesAll)
		{
			proceed = await DisplayAlert("Poista kaikki sanastot?", 
				$"Haluatko varmasti poistaa kaikki {e.ItemsToDelete.Length} sanastoa lopullisesti.", "Jatka", "Peruuta");
		}
		else
		{
			string message = $"Haluatko varmasti poistaa {e.ItemsToDelete.Length} sanastoa lopullisesti? " +
				$"Painamalla 'Jatka', sanastot '{string.Join(", ", e.ItemsToDelete.Select(x => x.Name))}' poistetaan.";

			proceed = await DisplayAlert("Poista sanastoja", message, "Jatka", "Peruuta");
		}
		if (proceed)
		{
            await Model.DeleteCollections(e.ItemsToDelete);
        }
    }

    private async void Model_CollectionDeleted(object sender, WordDataAccessLibrary.DataBaseActions.DataBaseActionArgs e)
	{
		int amountDeleted = e.CollectionNames.Length;

        await DisplayAlert("Poistettu onnistuneesti!", $"{amountDeleted} sanastoa poistettiin onnistuneesti.", "OK");
    }

    public IWordCollectionHandlingViewModel Model => (IWordCollectionHandlingViewModel)BindingContext;

	private async void ContentPage_Loaded(object sender, EventArgs e)
	{
		await Model.ResetCollections();
	}


	private void HideMenu(object sender, EventArgs e)
	{
		menu.Collapse(sender, e);
	}
}