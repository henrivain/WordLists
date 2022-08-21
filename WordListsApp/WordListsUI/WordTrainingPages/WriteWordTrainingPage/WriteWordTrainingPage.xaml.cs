
namespace WordListsUI.WordTrainingPages.WriteWordTrainingPage;

public partial class WriteWordTrainingPage : ContentPage
{
	readonly GridHelper _gridHelper;

    public WriteWordTrainingPage(IWriteWordViewModel model)
	{
		BindingContext = model;
        InitializeComponent();
        _gridHelper = DeviceInfo.Current.Platform == DevicePlatform.WinUI ? new(baseGrid, infoVerticalStackLayout) : null;
    }

	public IWriteWordViewModel Model => (IWriteWordViewModel)BindingContext;

	private void Grid_SizeChanged(object sender, EventArgs e)
	{
		_gridHelper?.ReSize();
    }
}