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

	private static TesterCollection GetTestData()
	{
        Tester tester1 = new()
        {
            Word = "word1",
			LearnState = WordLearnState.NeverHeard
        };

        Tester tester2 = new()
        {
            Word = "word2",
            LearnState = WordLearnState.Learned
        };

        Tester tester3 = new()
        {
            Word = "word3",
            LearnState = WordLearnState.MightKnow
        };

        return new()
		{
			Description = new("This is collection"),
			Testers = { tester1, tester2, tester3 }
		};
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{


        int id = await TestService.InsertCollection(GetTestData());
		Debug.WriteLine("Collection id: " + id);

		TesterCollection collection = await TestService.GetByTag(id);



		Debug.WriteLine(collection.Testers.FirstOrDefault().Word);
		Debug.WriteLine(collection.Testers[2].LearnState);
		Debug.WriteLine(collection.Description.Name);

		
	}
}