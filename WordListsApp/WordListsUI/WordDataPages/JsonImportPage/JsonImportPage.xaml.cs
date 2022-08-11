using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.ActionResults;
using WordListsMauiHelpers.Factories;

namespace WordListsUI.WordDataPages.JsonImportPage;

public partial class JsonImportPage : ContentPage
{
	public JsonImportPage(IAbstractFactory<IJsonImportViewModel> modelFactory)
	{
        ModelFactory = modelFactory;
        BindingContext = ModelFactory.Create();
        ContentPage_BindingContextChanged(this, EventArgs.Empty);
        InitializeComponent();
    }


    public IJsonImportViewModel Model => (IJsonImportViewModel)BindingContext;
    private IAbstractFactory<IJsonImportViewModel> ModelFactory { get; }

    private async void Model_ImportSuccessfull(object sender, ImportActionResult e)
    {
        await DisplayAlert("Sanastot tuotu!", e.MoreInfo, "OK");
        BindingContext = ModelFactory.Create();
    }
    private async void Model_ImportActionFailed(object sender, ImportActionResult e)
    {
        bool accepted = await DisplayAlert("Jotain meni pieleen tuontiprocessissa :(", e.MoreInfo, "Poistu", "Yritä uudelleen");
        if (accepted) BindingContext = ModelFactory.Create();
    }
    private async void Model_EmptyImportAttempted(object sender, ImportActionResult e)
    {
        await DisplayAlert("Tiedostoa ei ole valittu", 
            "Tuotava tiedosto tulee olla valittu käyttämällä saatavilla olevaa nappia \"Valitse\"", 
            "OK");
    }
    private void Model_SelectFileAttempted(object sender, FileActionResult e)
    {
        if (e.Success is false)
        {
            ShowOpacityAnimation(wrongFileTypeLabel);
        }
    }
    private void ShowOpacityAnimation(View view)
    {
        Animation fadeIn = new(o =>
        { view.Opacity = o; }, 0, 1, Easing.CubicOut);

        Animation fadeOut = new(o =>
        { view.Opacity = o; }, 1, 0, Easing.CubicIn);

        Animation parent = new()
        {
            { 0, 0.2, fadeIn },
            { 0.2, 1, fadeOut }
        };
        parent.Commit(this, "opacityAnimation", 16, 6000);
    }

    private void HideMenu(object sender, EventArgs e)
    {
        menu.Collapse(sender, e);
    }
   

    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
    {
        Model.EmptyImportAttempted += Model_EmptyImportAttempted;
        Model.ImportActionFailed += Model_ImportActionFailed;
        Model.ImportSuccessfull += Model_ImportSuccessfull;
        Model.SelectFileAttempted += Model_SelectFileAttempted;
    }
}