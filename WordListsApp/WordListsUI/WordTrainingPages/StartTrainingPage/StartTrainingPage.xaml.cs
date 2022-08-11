using System.Diagnostics;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordListsMauiHelpers.PageRouting;

namespace WordListsUI.WordTrainingPages.StartTrainingPage;

public partial class StartTrainingPage : ContentPage
{
	public StartTrainingPage(IStartTrainingViewModel model, IWordCollectionService collectionService)
	{
		BindingContext = model;
        InitializeComponent();
		CollectionService = collectionService;
	}

    public IStartTrainingViewModel Model => (IStartTrainingViewModel)BindingContext;

	public IWordCollectionService CollectionService { get; }

	private async void ContentPage_Loaded(object sender, EventArgs e)
	{
		await Model.ResetCollections();
	}

	private async void SwipeItem_Clicked(object sender, EventArgs e)
	{
		int id = -1;

		if (sender is MenuItem item)
		{
            id = (int)item.CommandParameter;
        }

		if (sender is ImageButton imgButton)
		{
			id = (int)imgButton.CommandParameter;
		}

		if (id is not -1)
		{
			await Shell.Current.GoToAsync($"{PageRoutes.GetRoute(Route.Training)}/{nameof(FlipCardTrainingPage.FlipCardTrainingPage)}", await BuildPageChangeParameter(id));
		}
	}

	private async Task<Dictionary<string, object>> BuildPageChangeParameter(int id)
	{
		WordCollection collection = await CollectionService.GetWordCollection(id);

		if (Model.ShowLearnedWords is false)
		{
			collection.WordPairs = collection.WordPairs
				.Where(x => x.LearnState is not WordLearnState.Learned )
					.ToList();
		}
		if (Model.ShowMightKnowWords is false)
		{
            collection.WordPairs = collection.WordPairs
				.Where(x => x.LearnState is not WordLearnState.MightKnow)
					.ToList();
        }
		if (Model.ShowNeverHeardKnowWords is false)
		{
            collection.WordPairs = collection.WordPairs
				.Where(x => x.LearnState is not WordLearnState.NeverHeard)
					.ToList();	
        }

		var parameter = new Dictionary<string, object>()
		{
			["StartCollection"] = collection
		};
		return parameter;
	}

	private void SearchField_SizeChanged(object sender, EventArgs e)
	{
		if (sender is StackLayout layout)
		{
			Debug.WriteLine(layout.Width);
		}
	}
}