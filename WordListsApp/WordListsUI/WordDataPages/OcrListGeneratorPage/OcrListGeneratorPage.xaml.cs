using WordDataAccessLibrary;
using WordListsMauiHelpers.DependencyInjectionExtensions;
using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordDataPages.ListGeneratorPage;
using WordListsViewModels.Events;

namespace WordListsUI.WordDataPages.OcrListGeneratorPage;

public partial class OcrListGeneratorPage : ContentPage
{
    public OcrListGeneratorPage(IAbstractFactory<IOcrListGeneratorViewModel> vmFactory, ILogger<ContentPage> logger)
    {
        Logger = logger;
        ViewModelFactory = vmFactory;
        BindingContext = vmFactory.Create();
        InitializeComponent();
        BindingContextChanged += OcrListGeneratorPage_BindingContextChanged;
    }
    IOcrListGeneratorViewModel Model => (IOcrListGeneratorViewModel)BindingContext;
    IAbstractFactory<IOcrListGeneratorViewModel> ViewModelFactory { get; }
    public ILogger<ContentPage> Logger { get; }

    private void ResetViewModel_Clicked(object sender, EventArgs e)
    {
        BindingContext = ViewModelFactory.Create();
    }

    private void OcrListGeneratorPage_BindingContextChanged(object? sender, EventArgs e)
    {
        Model.ParseFailed += ViewModel_ParseFailed;
        Model.RecognizionFailed += ViewModel_RecognizionFailed;
        Model.NoTextWasRecognized += ViewModel_NoTextWasRecognized;
    }

    private async void ViewModel_NoTextWasRecognized(object sender, TesseractFailedEventArgs e)
    {
        string msg = """
            Kuvasta ei havaittu yht��n teksti�.
            Kuva saattaa olla liian huono tai teksti liian ep�selv�.
            Tarkista my�s, ett� kuvassa on teksti� ja ett� teksti on hyvin valaistu.
            """;
        await DisplayAlert("Teksti� ei havaittu.", msg, "OK");
    }


    private async void ViewModel_RecognizionFailed(object sender, TesseractFailedEventArgs e)
    {
        string msg = $"""
            Teksti� ei pystytty havaitsemaan.
            Kuva ei ehk� en�� ole olemassa tai se on v��r�ss� muodossa.
            Tarkka syy:
            {e.Output?.Message}
            '{e.Message}'
            """;
        await DisplayAlert("Havaitseminen ep�oonistui.", msg, "OK");
    }

    private async void ViewModel_ParseFailed(object sender, string error)
    {
        string msg = $"""
            Teksti� ei pystytty parsimaan.
            Teksti saattoi olla tyhj� tai parsimiss��nn�t eiv�t olleet oikein.
            Tarkka syy:
            {error}
            """;
        await DisplayAlert("Parsimisvirhe.", msg, "OK");
    }

    private async void ProceedToEdit_Clicked(object sender, EventArgs e)
    {
        Logger.LogInformation("Proceeding to edit ocr generated word list.");

        if (Model.Pairs.Count <= 0)
        {
            Logger.LogInformation("No pairs to edit, can't proceed");
            string msg = "Lis�� sanoja, jotta voit siirty� muokkaamaan.";
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