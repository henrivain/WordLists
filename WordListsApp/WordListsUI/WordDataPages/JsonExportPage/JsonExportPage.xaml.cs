using WordDataAccessLibrary.CollectionBackupServices;
using WordListsMauiHelpers.DependencyInjectionExtensions;
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
        await DisplayAlert("Sanastoja ei valittu!", "Et voi vied‰ tyhj‰‰ sanastokokonaisuutta", "OK");
    }
    private async void Model_ExportCompleted(object sender, ExportActionResult e)
    {
        if (e.Success)
        {
            await DisplayAlert("Sanastot viety!", $"Sanastot on viety onnistuneesti hakemistoon: {e.UsedPath}", "OK");
            BindingContext = ModelFactory.Create();
            return;
        }
        bool accepted = await DisplayAlert("Jotain meni pieleen :(", $"Vienti ep‰onnistui, syy: \"{e.MoreInfo}\"", "Poistu", "Yrit‰ uudelleen");
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

    private async void ChangeFileName_Clicked(object sender, EventArgs e)
    {
        string result;
        bool isFirstAttempt = true;
        do
        {
            string title = isFirstAttempt ? "Vaihda tiedostonimi" 
                : "Anna kelvollinen tiedostonimi";
            string msg = isFirstAttempt ? "Anna haluamasi nimi viet‰v‰lle tiedostolle."
                : "Nime‰ ei voi j‰tt‰‰ tyhj‰ksi. Lis‰‰ nimi tai paina peruuta.";

            result = await DisplayPromptAsync(title,
               msg, "OK", "Peruuta", "tiedoston nimi", 48, null, Model.ExportFileName);

            if (result is null)
            {
                return;
            }
            isFirstAttempt = false;
        }
        while (string.IsNullOrWhiteSpace(result));

        Model.ExportFileName = string.Join("_", 
            result.Split(Path.GetInvalidFileNameChars()));
    }
}