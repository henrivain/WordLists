using System.Diagnostics;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;

namespace WordListsUI.ListGeneratorPage;

public partial class ListGeneratorPage : ContentPage
{
	public ListGeneratorPage()
	{
		InitializeComponent();
	}

    private static WordCollection GetTestData() => new()
    {
        Owner = new()
        {
            Name = "WordCollection1",
            Description = "this is WordCollection1",
            Id = 1
        },
        WordPairs = new()
        {
            new()
            {
                NativeLanguageWord = "a Tree",
                ForeignLanguageWord = "puu",
                IndexInVocalbulary = 0,
                LearnState = WordLearnState.MightKnow,
                OwnerId = 1
            },
            new()
            {
                NativeLanguageWord = "a flower",
                ForeignLanguageWord = "kukka",
                IndexInVocalbulary = 1,
                LearnState = WordLearnState.NeverHeard,
                OwnerId = 1
            },
            new()
            {
                NativeLanguageWord = "a pig",
                ForeignLanguageWord = "sika",
                IndexInVocalbulary = 2,
                LearnState = WordLearnState.Learned,
                OwnerId = 1
            },
            new()
            {
                NativeLanguageWord = "a car",
                ForeignLanguageWord = "auto",
                IndexInVocalbulary = 3,
                LearnState = WordLearnState.NeverHeard,
                OwnerId = 1
            }

        }
    };

    private async void Button_Clicked(object sender, EventArgs e)
	{
        var d = GetTestData();
        int id = await WordCollectionService.AddWordCollection(d);
        var n = await WordCollectionService.GetWordCollection(id);
        Debug.WriteLine(n.Owner.Name);


	}
}