using WordListsMauiHelpers.Factories;
using WordListsUI.Helpers;
using WordDataAccessLibrary.DataBaseActions;
using WordListsViewModels.Interfaces;

namespace WordListsUI.WordDataPages.ListGeneratorPage;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListGeneratorPage : ContentPage
{
	public ListGeneratorPage(IAbstractFactory<IListGeneratorViewModel> modelFactory)
	{
		ModelFactory = modelFactory;
		BindingContext = modelFactory.Create();
		ContentPage_BindingContextChanged(this, EventArgs.Empty);
		InitializeComponent();
    }


    IAbstractFactory<IListGeneratorViewModel> ModelFactory { get; }

    public IListGeneratorViewModel Model => (IListGeneratorViewModel)BindingContext;
	
	private void ITextInput_Focused(object sender, FocusEventArgs e)
	{
		if (sender is ITextInput input)
		{
			// this does not work with editor
            UIInteractionHelper.FocusITextInputText(input, this);
		}
	}
	private async void Model_CollectionAddedEvent(object sender, DataBaseActionArgs e)
	{
		BindingContext = ModelFactory.Create();
		await DisplayAlert("Onnistui!", $"Sanasto lisätty onnistuneesti säilytykseen id:llä {e.RefId}", "OK");
    }
    private void StartAgainBtn_Click(object sender, EventArgs e)
	{
		BindingContext = ModelFactory.Create();
	}
	private void HideMenu(object sender, EventArgs e)
	{
		menu.Collapse(sender, e);
	}
	private void ContentPage_BindingContextChanged(object sender, EventArgs e)
	{
        Model.CollectionAddedEvent += Model_CollectionAddedEvent;
    }

	
}