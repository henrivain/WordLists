using WordListsMauiHelpers.Factories;
using WordListsUI.Helpers;
using WordListsViewModels;
using WordDataAccessLibrary.DataBaseActions;

namespace WordListsUI.ListGeneratorPage;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListGeneratorPage : ContentPage
{
	public ListGeneratorPage(IAbstractFactory<IListGeneratorViewModel> modelFactory)
	{
		ModelFactory = modelFactory;
		BindingContext = modelFactory.Create(); ;
		InitializeComponent();
		Model.CollectionAddedEvent += Model_CollectionAddedEvent;
	}

	private async void Model_CollectionAddedEvent(object sender, DataBaseActionArgs e)
	{
		BindingContext = ModelFactory.Create();
		await DisplayAlert("Onnistui!", $"Sanasto lisätty onnistuneesti säilytykseen id:llä {e.RefId}", "OK");
	}

	IAbstractFactory<IListGeneratorViewModel> ModelFactory { get; }

    public IListGeneratorViewModel Model => (IListGeneratorViewModel)BindingContext;
	
	private void ITextInput_Focused(object sender, FocusEventArgs e)
	{
		if (sender is ITextInput input)
		{
			// this does not work with editor
            UiInteractionHelper.FocusITextInputText(input, this);
		}
	}

	
	private void StartAgainBtn_Click(object sender, EventArgs e)
	{
		BindingContext = ModelFactory.Create();
	}
}