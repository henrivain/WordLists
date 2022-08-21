using WordDataAccessLibrary;

namespace WordListsUI.WordTrainingPages.WritingTestPage;

[QueryProperty(nameof(StartCollection), nameof(StartCollection))]
public partial class WritingTestConfigurationPage : ContentPage
{
    public WritingTestConfigurationPage(IWritingTestConfigurationViewModel model)
    {
        InitializeComponent();
        Model = model;
    }

    public WordCollection StartCollection { get; set; }
    public IWritingTestConfigurationViewModel Model { get; }

    
}