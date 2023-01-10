using WordDataAccessLibrary.CollectionBackupServices;
using WordListsMauiHelpers.Factories;
using WordListsUI.Helpers;

namespace WordListsUI.WordDataPages.JsonExportPage;

public partial class JsonExportPage : ContentPage
{
    public JsonExportPage(IAbstractFactory<IJsonExportViewModel> modelFactory)
    {
        ModelFactory = modelFactory;
        BindingContext = ModelFactory.Create();
        ContentPage_BindingContextChanged(this, EventArgs.Empty);
        InitializeComponent();
    }

    private async void Model_EmptyExportAttempted(object sender, ExportActionResult e)
    {
        await DisplayAlert("Sanastoja ei valittu!", "Et voi viedä tyhjää sanastokokonaisuutta", "OK");
    }
    private async void Model_ExportCompleted(object sender, ExportActionResult e)
    {
        if (e.Success)
        {
            await DisplayAlert("Sanastot viety!", $"Sanastot on viety onnistuneesti hakemistoon: {e.UsedPath}", "OK");
            BindingContext = ModelFactory.Create();
            return;
        }
        bool accepted = await DisplayAlert("Jotain meni pieleen :(", $"Vienti epäonnistui, syy: \"{e.MoreInfo}\"", "Poistu", "Yritä uudelleen");
        if (accepted) BindingContext = ModelFactory.Create();
    }
    public IJsonExportViewModel Model => (IJsonExportViewModel)BindingContext;
    private IAbstractFactory<IJsonExportViewModel> ModelFactory { get; }
    private void ITextInput_Focused(object sender, FocusEventArgs e)
    {
        if (sender is ITextInput input)
        {
            // this does not work with editor
            UIInteractionHelper.FocusITextInputText(input, this);
        }
    }

    private void HideMenu(object sender, EventArgs e)
    {
        menu.Collapse(sender, e);
    }

    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
    {
        Model.ExportCompleted += Model_ExportCompleted;
        Model.EmptyExportAttempted += Model_EmptyExportAttempted;
    }
}