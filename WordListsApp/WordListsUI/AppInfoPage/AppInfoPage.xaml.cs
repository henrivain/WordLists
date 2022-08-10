namespace WordListsUI.AppInfoPage;

public partial class AppInfoPage : ContentPage
{
	public AppInfoPage(IAppInfoViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
	}

	public IAppInfoViewModel Model => (IAppInfoViewModel)BindingContext;
}