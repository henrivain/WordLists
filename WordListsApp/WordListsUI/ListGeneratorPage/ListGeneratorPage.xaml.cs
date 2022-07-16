using System.Diagnostics;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.Generators;
using WordListsViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using InputView = Microsoft.Maui.Controls.InputView;
using WordListsUI.Helpers;

namespace WordListsUI.ListGeneratorPage;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListGeneratorPage : ContentPage
{
	public ListGeneratorPage(IListGeneratorViewModel model)
	{
        BindingContext = model;
		InitializeComponent();
	}

    public IListGeneratorViewModel ViewModel => (IListGeneratorViewModel)BindingContext;
	
	private void ITextInput_Focused(object sender, FocusEventArgs e)
	{
		if (sender is ITextInput input)
		{
			// this does not work with editor
            UiInteractionHelper.FocusITextInputText(input, this);
		}
	}

}