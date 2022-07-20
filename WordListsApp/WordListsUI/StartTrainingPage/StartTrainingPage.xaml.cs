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
}