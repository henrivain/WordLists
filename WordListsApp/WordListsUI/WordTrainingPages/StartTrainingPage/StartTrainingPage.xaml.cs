using Microsoft.Maui.Controls;
using System.Diagnostics;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordTrainingPages.WritingTestPage;

namespace WordListsUI.WordTrainingPages.StartTrainingPage;

public partial class StartTrainingPage : ContentPage
{
	public StartTrainingPage(IStartTrainingViewModel model, IWordCollectionService collectionService)
	{
		BindingContext = model;
        InitializeComponent();
		CollectionService = collectionService;
		Model.WriteTrainingRequestedEvent += Model_WriteTrainingRequestedEvent;
		Model.CardsTrainingRequestedEvent += Model_CardsTrainingRequestedEvent;
	}

	private async void Model_CardsTrainingRequestedEvent(object sender, WordListsViewModels.Events.StartTrainingEventArgs e)
	{
		if (e.WordCollection is null)
		{
			Debug.WriteLine($"{nameof(StartTrainingPage)} Cannot navigate to training page, because given parameter {nameof(e.WordCollection)} is null");
		}

		await Shell.Current.GoToAsync($"{PageRoutes.GetRoute(Route.Training)}/{nameof(FlipCardTrainingPage.FlipCardTrainingPage)}", new Dictionary<string, object>()
		{
            ["StartCollection"] = e.WordCollection
        });

    }

    private async void Model_WriteTrainingRequestedEvent(object sender, WordListsViewModels.Events.StartTrainingEventArgs e)
	{
        if (e.WordCollection is null)
		{
			Debug.WriteLine($"{nameof(StartTrainingPage)} Cannot navigate to training page, because given parameter {nameof(e.WordCollection)} is null");
		}

		await Shell.Current.GoToAsync($"{PageRoutes.GetRoute(Route.Training)}/{nameof(WritingTestConfigurationPage)}", new Dictionary<string, object>()
        {
            ["StartCollection"] = e.WordCollection
        });
    }

    public IStartTrainingViewModel Model => (IStartTrainingViewModel)BindingContext;

	public IWordCollectionService CollectionService { get; }

	private async void ContentPage_Loaded(object sender, EventArgs e)
	{
		await Model.ResetCollections();
	}
}