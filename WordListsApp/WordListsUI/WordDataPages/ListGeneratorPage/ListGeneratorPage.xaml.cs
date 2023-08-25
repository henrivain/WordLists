#if WINDOWS
using Microsoft.UI.Xaml;
using Windows.System;
#endif
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordListsUI.Helpers;
using WordListsViewModels.Events;

namespace WordListsUI.WordDataPages.ListGeneratorPage;

[QueryProperty(nameof(PageModeParameter), nameof(PageModeParameter))]
public partial class ListGeneratorPage : ContentPage
{
    public ListGeneratorPage(IAbstractFactory<IListGeneratorViewModel> modelFactory)
    {
        ModelFactory = modelFactory;
        BindingContext = modelFactory.Create();
        ContentPage_BindingContextChanged(this, EventArgs.Empty);
        InitializeComponent();
    }





    private async void Model_EditFinished(object sender, DataBaseActionArgs e)
    {
        await DisplayAlert("P‰ivitetty onnistuneesti!",
            "Onnistuneesti poistettiin vanha sanasto ja lis‰ttiin sen uusi muokattu versio. " +
            "Huomaa, ett‰ n‰kym‰ pit‰‰ ehk‰ p‰ivitt‰‰, jotta muutettu sanasto n‰kyy oikein.", "OK");
        await Shell.Current.Navigation.PopAsync();
    }

    public PageModeParameter<ListGeneratorMode> PageModeParameter { set => UseMode(value); }
    private void UseMode(PageModeParameter<ListGeneratorMode> value)
    {
        switch (value)
        {
            case { Mode: ListGeneratorMode.Default }:
                return;

            case { Mode: ListGeneratorMode.Edit, Data: WordCollection collection }:
                Model.OpenInEditMode(collection);
                return;

            case { Mode: ListGeneratorMode.EditNew, Data: IEnumerable<string> words }:
                Model.ResetWordPairs(words.ToArray());
                return;

            default:
                throw new ArgumentException("Mode and Data combination not supported", nameof(value));
        }
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
        await DisplayAlert("Onnistui!", $"Sanasto lis‰tty onnistuneesti s‰ilytykseen nimell‰ '{e.NameString}'", "OK");
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
        Model.EditFinished += Model_EditFinished;
        Model.FailedToSaveEvent += Model_FailedToSaveEvent;
        Model.ParserError += Model_ParserError;
    }

    private async void Model_ParserError(object sender, string error)
    {
        await DisplayAlert("Parsinta ep‰onnistui",
            $"""
            Sanaston parsiminen ep‰onnistui. 
            Syy: '{error}'. 
            Jos vika toistuu, ota yhteytt‰ kehitt‰j‰‰n.
            """, "OK");
    }
    private async void Model_FailedToSaveEvent(object sender, DataBaseActionArgs e)
    {
        await DisplayAlert("Tallentaminen ep‰onnistui!", $"Sanaston tallentaminen ep‰onnistui. \n\nSyy: \n'{e.Text}'", "OK");
    }



    private async void OpenEditWantedDialog(object sender, EditEventArgs e)
    {
        var initValue = e.CurrentValue;

        string? result = await DisplayPromptAsync("Muokkaa sanaa", "kirjoita sanalle haluamasi muoto", "OK", "peruuta", "muokattu sana", initialValue: initValue);

        if (result is null)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(result))
        {
            Model.Delete.Execute(initValue);
            return;
        }

        Model.SetWordValueWithIndex(e.IndexInList, result);    //e.IndexInList
    }

    private async void OpenAddWordDialog(object sender, EventArgs e)
    {
        string? result = await DisplayPromptAsync("Lis‰‰ uusi sana", "kirjoita listan loppuun lis‰tt‰v‰ sana",
            "OK", "peruuta", "lis‰tt‰v‰ sana");

        if (string.IsNullOrEmpty(result))
        {
            return;
        }

        Model.AddWord(result);
    }



    private async void CancelBtn_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }


#if WINDOWS
    protected override void OnHandlerChanged()
    {
        if (Handler?.PlatformView is UIElement element)
        {
            element.ProcessKeyboardAccelerators += (sender, args) =>
            {
                switch (args.Modifiers, args.Key)
                {
                    case (VirtualKeyModifiers.Control, VirtualKey.N):
                        Model.New.Execute(null);
                        break;
                    case (VirtualKeyModifiers.Control, VirtualKey.S):
                        Model.Save.Execute(null);
                        break;
                }
            };
        }
    }
#endif
}