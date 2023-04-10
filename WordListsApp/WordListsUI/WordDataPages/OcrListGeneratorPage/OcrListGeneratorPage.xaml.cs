namespace WordListsUI.WordDataPages.OcrListGeneratorPage;

public partial class OcrListGeneratorPage : ContentPage
{
	public OcrListGeneratorPage(IOcrListGeneratorViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    public IOcrListGeneratorViewModel ViewModel => (IOcrListGeneratorViewModel)BindingContext;
}