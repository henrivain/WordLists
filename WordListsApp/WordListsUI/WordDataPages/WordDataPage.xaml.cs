namespace WordListsUI.WordDataPages;

public partial class WordDataPage : ContentPage
{
	public WordDataPage(IWordDataViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

	public IWordDataViewModel Model => (IWordDataViewModel)BindingContext;

	private void HideMenu(object sender, EventArgs e)
	{
		menu.Collapse(sender, e);
	}
}