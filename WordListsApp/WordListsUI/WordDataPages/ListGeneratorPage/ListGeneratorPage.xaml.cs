using WordListsMauiHelpers.Factories;
using WordListsUI.Helpers;
using WordDataAccessLibrary.DataBaseActions;
using WordListsViewModels.Interfaces;
using WordListsViewModels.Events;

namespace WordListsUI.WordDataPages.ListGeneratorPage;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListGeneratorPage : ContentPage
{
    public ListGeneratorPage(IAbstractFactory<IListGeneratorViewModel> modelFactory)
    {
        ModelFactory = modelFactory;
        BindingContext = modelFactory.Create();
        ContentPage_BindingContextChanged(this, EventArgs.Empty);
        InitializeComponent();
    }


    IAbstractFactory<IListGeneratorViewModel> ModelFactory { get; }

    public IListGeneratorViewModel Model => (IListGeneratorViewModel)BindingContext;

    private void ITextInput_Focused(object sender, FocusEventArgs e)
    {
        if (sender is ITextInput input)
        {
            // this does not work with editor
            UIInteractionHelper.FocusITextInputText(input, this);
        }
    }
    private async void Model_CollectionAddedEvent(object sender, DataBaseActionArgs e)
    {
        BindingContext = ModelFactory.Create();
        await DisplayAlert("Onnistui!", $"Sanasto lis‰tty onnistuneesti s‰ilytykseen id:ll‰ {e.RefId}", "OK");
    }
    private void StartAgainBtn_Click(object sender, EventArgs e)
    {
        BindingContext = ModelFactory.Create();
    }
    private void HideMenu(object sender, EventArgs e)
    {
        menu.Collapse(sender, e);
    }
    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
    {
        Model.CollectionAddedEvent += Model_CollectionAddedEvent;
        Model.EditWantedEvent += OpenEditWantedDialog;
        Model.AddWantedEvent += OpenAddWordDialog;
    }

    private async void OpenEditWantedDialog(object sender, EditWantedEventArgs e)
    {
        var result = await DisplayPromptAsync("Muokkaa sanaa", "kirjoita sanalle haluamasi muoto", "OK", "peruuta", "muokattu sana", initialValue: e.CurrentValue);
        if (string.IsNullOrEmpty(result)) return;
        Model.SetWordValueWithIndex(e.IndexInList, result);    //e.IndexInList
    }

    private async void OpenAddWordDialog(object sender, EventArgs e)
    {
        var result = await DisplayPromptAsync("Lis‰‰ uusi sana", "kirjoita listan loppuun lis‰tt‰v‰ sana", "OK", "peruuta", "lis‰tt‰v‰ sana");
        if (string.IsNullOrEmpty(result)) return;
        Model.AddWord(result);
    }
}