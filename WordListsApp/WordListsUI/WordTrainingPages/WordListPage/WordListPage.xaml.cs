namespace WordListsUI.WordTrainingPages.WordListPage;

public partial class WordListPage : ContentPage
{
	public WordListPage(IWordListViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
	}
	IWordListViewModel ViewModel => (IWordListViewModel)BindingContext;

}