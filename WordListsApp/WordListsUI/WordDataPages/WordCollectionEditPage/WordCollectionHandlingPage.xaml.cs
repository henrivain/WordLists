namespace WordListsUI.WordDataPages.WordCollectionEditPage;

public partial class WordCollectionEditPage : ContentPage
{
	public WordCollectionEditPage(IWordCollectionHandlingViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
        Model.CollectionDeleted += Model_CollectionDeleted;
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

	private async void DeleteCollection_Clicked(object sender, EventArgs e)
	{
        int id = -1;
		if (sender is MenuItem item)
		{
			id = (int)item.CommandParameter;
		}
		if (sender is ImageButton imgBtn)
		{
			id = (int)imgBtn.CommandParameter;
		}
		if (id is not -1)
		{
            bool proceed = await DisplayAlert(
				title:"Poista sanasto?",
				message: $"Haluatko varmasti poistaa sanaston: \" \"? id: {id}",
				cancel: "peruuta",
				accept: "jatka"
				);

            if (proceed)
			{
                await Model.DeleteCollection(id);
            }
        }
	}

	private void HideMenu(object sender, EventArgs e)
	{
		menu.Collapse(sender, e);
	}
}