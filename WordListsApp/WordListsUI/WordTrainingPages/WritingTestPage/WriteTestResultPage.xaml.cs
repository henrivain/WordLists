using WordListsUI.WordTrainingPages.WritingTestPage.GridManagement;
using WordListsViewModels.Helpers;

namespace WordListsUI.WordTrainingPages.WritingTestPage;

[QueryProperty(nameof(ProgressionSaved), nameof(ProgressionSaved))]
[QueryProperty(nameof(AnsweredQuestions), nameof(AnsweredQuestions))]
[QueryProperty(nameof(SessionId), nameof(SessionId))]
public partial class WriteTestResultPage : ContentPage
{
    readonly WriteTestResultGridHelper _gridHelper;
    public WriteTestResultPage(ITestResultViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
		Model.ExitResults += Model_ExitResults;
		if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
		{
			_gridHelper = new(baseGrid, infoView);
			baseGrid.MaximumWidthRequest = 1600;
			infoView.MaximumWidthRequest = 600;
			mainCollectionView.MaximumWidthRequest = 600;
        }

    }


    private async void Model_ExitResults(object sender, EventArgs e)
	{
		await Shell.Current.Navigation.PopToRootAsync();
	}

	public bool ProgressionSaved { set => Model.ProgressionSaved = value; }
	public List<WordPairQuestion> AnsweredQuestions { set { Model.AnsweredQuestions = value; } }

	public string SessionId { set { Model.SessionId = value; } }

    ITestResultViewModel Model => (ITestResultViewModel)BindingContext;

	private void BaseGrid_SizeChanged(object sender, EventArgs e) => _gridHelper?.ReSize();
}