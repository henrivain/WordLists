namespace WordListsUI.JsonExportPage;

public partial class JsonExportPage : ContentPage
{
	public JsonExportPage(IJsonExportViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
	}

	public IJsonExportViewModel Model => (IJsonExportViewModel)BindingContext;
}