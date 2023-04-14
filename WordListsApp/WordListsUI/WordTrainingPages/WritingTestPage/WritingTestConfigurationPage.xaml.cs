using WordDataAccessLibrary;
using WordListsMauiHelpers.DependencyInjectionExtensions;
using WordListsMauiHelpers.PageRouting;
using WordListsViewModels.Events;

namespace WordListsUI.WordTrainingPages.WritingTestPage;

[QueryProperty(nameof(StartCollection), nameof(StartCollection))]
public partial class WritingTestConfigurationPage : ContentPage
{
    public WritingTestConfigurationPage(IAbstractFactory<IWritingTestConfigurationViewModel> modelFactory, ILogger<ContentPage> logger)
    {
        InitializeComponent();
        ModelFactory = modelFactory;
        Logger = logger;
        NewBindingContext();
    }

    private async void Model_StartWordCollection(object sender, TestStartEventArgs e)
    {
        if (e.WordCollection is null)
        {
            Logger.LogError("{cls} Cannot navigate to training page, because event argument '{arg}' is null",
                nameof(WritingTestConfigurationPage), nameof(e.WordCollection));
            string msg = """
                Valitulla sanastolla ei ole arvoa, joten sit‰ ei voi harjoitella.
                Sanasto on ehk‰ ehditty poistaa. 
                P‰ivit‰ n‰kym‰ ja kokeile uudelleen.
                Jos ongelma toistuu, ota yhteytt‰ kehitt‰j‰‰n.
                """;
            await DisplayAlert("Tapahtui virhe", msg, "OK");
            return;
        }

        var path = $"{PageRoutes.Get(Route.Training)}/{nameof(WritingTestPage)}";
        await Shell.Current.Navigation.PopAsync();
        await Shell.Current.GoToAsync(path, new Dictionary<string, object>()
        {
            [nameof(WritingTestPage.StartCollection)] = e.WordCollection,
            [nameof(WritingTestPage.SaveProgression)] = e.SaveProgression
        });
        NewBindingContext();
    }
    public WordCollection StartCollection { set => Model.Collection = value; }
    public IWritingTestConfigurationViewModel Model => (IWritingTestConfigurationViewModel)BindingContext;
    IAbstractFactory<IWritingTestConfigurationViewModel> ModelFactory { get; }
    public ILogger<ContentPage> Logger { get; }

    private void NewBindingContext()
    {
        BindingContext = ModelFactory.Create();
        Model.StartWordCollection += Model_StartWordCollection;
    }

}