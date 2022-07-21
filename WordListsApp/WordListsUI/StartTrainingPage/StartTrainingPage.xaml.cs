using System.Diagnostics;
using WordDataAccessLibrary;
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

	private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection is WordCollectionOwner owner)
		{
			Model.SelectedItem = owner;
			Debug.WriteLine("yesyesyesyesyesyesyes");
		}
	}
}