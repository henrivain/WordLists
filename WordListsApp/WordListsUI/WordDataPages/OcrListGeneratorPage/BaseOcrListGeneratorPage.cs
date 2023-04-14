using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsViewModels.Events;

namespace WordListsUI.WordDataPages.OcrListGeneratorPage;

/// <summary>
/// Base type for the OcrListGeneratorPage or PhoneOcrListGeneratorPage.  
/// This type is registered as a app route and will give platform specific implementation of the OcrListGeneratorPage.
/// </summary>
public class BaseOcrListGeneratorPage : ContentPage
{
	protected BaseOcrListGeneratorPage (IOcrListGeneratorViewModel viewModel, ILogger<ContentPage> logger)
	{
        Logger = logger;
        BindingContext = viewModel;
        Title = "Luo kuvasta";
    }

    protected virtual IOcrListGeneratorViewModel Model => (IOcrListGeneratorViewModel)BindingContext;
    protected virtual ILogger<ContentPage> Logger { get; }

    protected virtual async void ViewModel_NoTextWasRecognized(object sender, TesseractFailedEventArgs e)
    {
        string msg = """
            Kuvasta ei havaittu yhtään tekstiä.
            Kuva saattaa olla liian huono tai teksti liian epäselvä.
            Tarkista myös, että kuvassa on tekstiä ja että teksti on hyvin valaistu.
            """;
        await DisplayAlert("Tekstiä ei havaittu.", msg, "OK");
    }

    protected virtual async void ViewModel_RecognizionFailed(object sender, TesseractFailedEventArgs e)
    {
        string msg = $"""
            Tekstiä ei pystytty havaitsemaan.
            Kuva ei ehkä enää ole olemassa tai se on väärässä muodossa.
            Tarkka syy:
            {e.Output?.Message}
            '{e.Message}'
            """;
        await DisplayAlert("Havaitseminen epäoonistui.", msg, "OK");
    }

    protected virtual async void ViewModel_ParseFailed(object sender, string error)
    {
        string msg = $"""
            Tekstiä ei pystytty parsimaan.
            Teksti saattoi olla tyhjä tai parsimissäännöt eivät olleet oikein.
            Tarkka syy:
            {error}
            """;
        await DisplayAlert("Parsimisvirhe.", msg, "OK");
    }

    protected virtual async void ProceedToEdit_Clicked(object sender, EventArgs e)
    {
        Logger.LogInformation("Proceeding to edit ocr generated word list.");

        if (Model.Pairs.Count <= 0)
        {
            Logger.LogInformation("No pairs to edit, can't proceed");
            string msg = "Lisää sanoja, jotta voit siirtyä muokkaamaan.";
            await DisplayAlert("Ei sanoja", msg, "OK");
            return;
        }

        List<string> words = new();
        foreach (var pair in Model.Pairs)
        {
            words.Add(pair.NativeLanguageWord);
            words.Add(pair.ForeignLanguageWord);
        }

        var queryParams = new Dictionary<string, object>()
        {
            [nameof(ListGeneratorPage.ListGeneratorPage.PageModeParameter)] = new PageModeParameter<ListGeneratorMode>()
            {
                Mode = ListGeneratorMode.EditNew,
                Data = words
            }
        };

        string route = $"{PageRoutes.Get(Route.WordHandling)}/{PageRoutes.Get(Route.LifeTime)}/{nameof(ListGeneratorPage.ListGeneratorPage)}";
        await Shell.Current.GoToAsync(route, queryParams);
    }
}