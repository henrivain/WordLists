using WordDataAccessLibrary;

namespace WordListsUI.WordTrainingPages.WritingTestPage;

[QueryProperty(nameof(StartCollection), nameof(StartCollection))]
public partial class WritingTestPage : ContentPage
{
	readonly WriteWordPageGridHelper _gridHelper;

    public WritingTestPage(IWriteWordViewModel model)
	{
		BindingContext = model;
        InitializeComponent();
        _gridHelper = DeviceInfo.Current.Platform == DevicePlatform.WinUI ? new(baseGrid, infoVerticalStackLayout) : null;
    }

    public WordCollection StartCollection
    {
        set
        {
            if (value is not null) Model.StartNew(value);
        }
    }

    public IWriteWordViewModel Model => (IWriteWordViewModel)BindingContext;

	private void Grid_SizeChanged(object sender, EventArgs e)
	{
		_gridHelper?.ReSize();
    }



	private async void LeaveButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.Navigation.PopToRootAsync();
    }
}