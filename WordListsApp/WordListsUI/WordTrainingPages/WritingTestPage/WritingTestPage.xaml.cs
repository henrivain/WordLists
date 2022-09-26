using WordDataAccessLibrary;
using WordListsMauiHelpers.Factories;

namespace WordListsUI.WordTrainingPages.WritingTestPage;

[QueryProperty(nameof(StartCollection), nameof(StartCollection))]
public partial class WritingTestPage : ContentPage
{
	readonly WriteWordPageGridHelper _gridHelper;

    public WritingTestPage(IAbstractFactory<IWriteWordViewModel> factory)
	{
        BindingContext = factory.Create();
        InitializeComponent();
        Factory = factory;
        _gridHelper = DeviceInfo.Current.Platform == DevicePlatform.WinUI ? new(baseGrid, infoVerticalStackLayout) : null;
    }

    public WordCollection StartCollection { set { if (value is not null) Model.StartNew(value); } }


    IWriteWordViewModel Model => (IWriteWordViewModel)BindingContext;

    IAbstractFactory<IWriteWordViewModel> Factory { get; }

    private void Grid_SizeChanged(object sender, EventArgs e)
	{
        _gridHelper?.ReSize();
    }



	private async void LeaveButton_Clicked(object sender, EventArgs e)
	{
        await Shell.Current.Navigation.PopAsync();
        BindingContext = Factory.Create();
        //await Shell.Current.Navigation.PopToRootAsync();  // This stopped working ?? don't know why, but it can be used if needed later
    }
}