//using CommunityToolkit.Maui.Behaviors;

using WordListsUI.Helpers;

namespace WordListsUI.JsonExportPage;

public partial class JsonExportPage : ContentPage
{
	public JsonExportPage(IJsonExportViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
	}

	public IJsonExportViewModel Model => (IJsonExportViewModel)BindingContext;

	private void ITextInput_Focused(object sender, FocusEventArgs e)
	{
        if (sender is ITextInput input)
        {
            // this does not work with editor
            UIInteractionHelper.FocusITextInputText(input, this);
        }
    }
}