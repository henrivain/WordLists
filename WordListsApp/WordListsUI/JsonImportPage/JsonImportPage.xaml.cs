using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.JsonServices;
using WordListsMauiHelpers.Factories;

namespace WordListsUI.JsonImportPage;

public partial class JsonImportPage : ContentPage
{
	public JsonImportPage(IAbstractFactory<IJsonImportViewModel> modelFactory)
	{
        ModelFactory = modelFactory;
        BindingContext = ModelFactory.Create();
        InitializeComponent();
        Model.EmptyImportAttempted += Model_EmptyImportAttempted;
        Model.ImportActionFailed += Model_ImportActionFailed;
        Model.ImportSuccessfull += Model_ImportSuccessfull; ;
    }

    private async void Model_ImportSuccessfull(object sender, ImportActionResult e)
    {
        await DisplayAlert("Sanastot tuotu!", e.MoreInfo, "OK");

    }

    private async void Model_ImportActionFailed(object sender, ImportActionResult e)
    {
        await DisplayAlert("Jotain meni pieleen tuontiprocessissa :(", e.MoreInfo, "OK");
    }

    private async void Model_EmptyImportAttempted(object sender, ImportActionResult e)
    {
        await DisplayAlert("Tiedostoa ei ole valittu", 
            "Tuotava tiedosto tulee olla valittu käyttämällä saatavilla olevaa nappia \"Valitse\"", 
            "OK");
    }

    public IJsonImportViewModel Model => (IJsonImportViewModel)BindingContext;
    private IAbstractFactory<IJsonImportViewModel> ModelFactory { get; }
}