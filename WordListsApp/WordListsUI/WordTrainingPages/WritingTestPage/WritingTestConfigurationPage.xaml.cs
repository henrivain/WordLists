using WordDataAccessLibrary;
using WordListsMauiHelpers.DependencyInjectionExtensions;
using WordListsMauiHelpers.PageRouting;
using WordListsViewModels.Events;

namespace WordListsUI.WordTrainingPages.WritingTestPage;

[QueryProperty(nameof(StartCollection), nameof(StartCollection))]
public partial class WritingTestConfigurationPage : ContentPage
{
    public WritingTestConfigurationPage(IAbstractFactory<IWritingTestConfigurationViewModel> modelFactory)
    {
        InitializeComponent();
        ModelFactory = modelFactory;
        NewBindingContext();
    }

    private async void Model_StartWordCollection(object sender, TestStartEventArgs e)
    {
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
    private void NewBindingContext()
    {
        BindingContext = ModelFactory.Create();
        Model.StartWordCollection += Model_StartWordCollection;
    }

}