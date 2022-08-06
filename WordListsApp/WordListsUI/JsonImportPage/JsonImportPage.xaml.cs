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
    }
    public IJsonImportViewModel Model => (IJsonImportViewModel)BindingContext;
    private IAbstractFactory<IJsonImportViewModel> ModelFactory { get; }
}