namespace WordListsUI.WordDataPages;

public partial class WordDataPage : ContentPage
{
	public WordDataPage()
	{
		InitializeComponent();
	}


	private void HideMenu(object sender, EventArgs e)
	{
		menu.Collapse(sender, e);
	}
}