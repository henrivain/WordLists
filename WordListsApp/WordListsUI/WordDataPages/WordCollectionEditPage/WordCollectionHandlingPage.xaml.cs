namespace WordListsUI.WordDataPages.WordCollectionEditPage;

public partial class WordCollectionEditPage : ContentPage
{
	public WordCollectionEditPage(IWordCollectionHandlingViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
        Model.CollectionDeleted += Model_CollectionDeleted;
		Model.DeleteRequested += Model_DeleteRequested;
    }

	private async void Model_DeleteRequested(object sender, WordListsViewModels.Events.DeleteWantedEventArgs e)
	{
        bool proceed = await DisplayAlert(
                title: "Poista sanasto?",
                message: $"Haluatko varmasti poistaa sanaston: '{e.WordCollectionOwner.Name}', id {e.WordCollectionOwner.Id}",
                cancel: "peruuta",
                accept: "jatka"
                );
		if (proceed)
		{
            await Model.DeleteCollection(e.WordCollectionOwner);
        }
    }

    private async void Model_CollectionDeleted(object sender, WordDataAccessLibrary.DataBaseActions.DataBaseActionArgs e)
	{
        await DisplayAlert("Poistettu onnistuneest!", $"Sanasto id:llä: {e.RefId} poistettiin onnistuneesti", "OK");
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