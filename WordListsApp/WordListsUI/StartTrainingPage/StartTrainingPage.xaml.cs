using WordDataAccessLibrary.DataBaseActions;
using WordListsViewModels;

namespace WordListsUI.StartTrainingPage;

public partial class StartTrainingPage : ContentPage
{
	public StartTrainingPage(IStartTrainingViewModel model)
	{
		BindingContext = model;
        InitializeComponent();
    }

    public IStartTrainingViewModel Model => (IStartTrainingViewModel)BindingContext;

	private async void ContentPage_Loaded(object sender, EventArgs e)
	{
		await Model.ResetCollections();
	}

	private async void SwipeItem_Clicked(object sender, EventArgs e)
	{
		int id = (int)((SwipeItem)sender).CommandParameter;
        await Shell.Current.GoToAsync($"{nameof(WordTrainingPage.WordTrainingPage)}?StartWordCollection={id}");
    }
}