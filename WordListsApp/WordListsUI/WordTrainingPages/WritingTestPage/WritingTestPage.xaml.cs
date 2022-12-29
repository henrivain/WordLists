using WordDataAccessLibrary;
using WordListsMauiHelpers.Factories;
using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordTrainingPages.WritingTestPage.GridManagement;
using WordListsViewModels.Events;

namespace WordListsUI.WordTrainingPages.WritingTestPage;

[QueryProperty(nameof(StartCollection), nameof(StartCollection))]
[QueryProperty(nameof(SaveProgression), nameof(SaveProgression))]
public partial class WritingTestPage : ContentPage
{
	readonly WordTrainingPageGridHelper _gridHelper;

    public WritingTestPage(IAbstractFactory<IWriteWordViewModel> factory)
	{
        Factory = factory;
        SetNewBindingContext();
        InitializeComponent();
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            _gridHelper = new(baseGrid, infoVerticalStackLayout);
            baseGrid.MaximumWidthRequest = 1400;
        }
    }

    public WordCollection StartCollection { set { if (value is not null) Model.StartNew(value); } }

    public bool SaveProgression { set => Model.SaveProgression = value; }

    IWriteWordViewModel Model => (IWriteWordViewModel)BindingContext;

    IAbstractFactory<IWriteWordViewModel> Factory { get; }

    private void Grid_SizeChanged(object sender, EventArgs e) => _gridHelper?.ReSize();

    private async void LeaveButton_Clicked(object sender, EventArgs e)
	{
        //await Shell.Current.Navigation.PopToRootAsync();  // This stopped working ?? don't know why, but it can be used if needed later
        await Shell.Current.Navigation.PopAsync();
    }


    private void SetNewBindingContext()
    {
        BindingContext = Factory.Create();
        Model.TestValidated += Model_TestValidated;
    }

    private async void Model_TestValidated(object sender, TestValidatedEventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
        await Shell.Current.GoToAsync($"{PageRoutes.GetRoute(Route.Training)}/{nameof(WriteTestResultPage)}", new Dictionary<string, object>()
        {
            [nameof(WriteTestResultPage.AnsweredQuestions)] = e.Questions,
            [nameof(WriteTestResultPage.SessionId)] = e.SessionId,
            [nameof(WriteTestResultPage.ProgressionSaved)] = e.ProgressionSaved,
        });
    }
}