using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime;
using WordDataAccessLibrary;
using WordListsMauiHelpers.Factories;
using WordListsMauiHelpers.PageRouting;

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

    private async void Model_StartWordCollection(object sender, WordCollection collection)
    {
        var parameter = new Dictionary<string, object>()
        {
            [nameof(WritingTestPage.StartCollection)] = collection
        };

        //NewBindingContext();

        var path = $"{PageRoutes.GetRoute(Route.Training)}/{nameof(WritingTestPage)}";
       
        await Shell.Current.GoToAsync(path, new Dictionary<string, object>()
        {
            ["StartCollection"] = collection
        });
       
        
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