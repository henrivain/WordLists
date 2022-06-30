using Microsoft.Maui.Controls;
using WordListsUI.WordTrainingPage.FlipCardControl;

namespace WordListsUI.WordTrainingPage;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class WordTrainingPage : ContentPage
{
	public WordTrainingPage()
	{
		InitializeComponent();
	}

	private void FlipCard_Loaded(object sender, EventArgs e)
	{
		flipper.WordPair = new()
		{
			NativeLanguageWord = "this is changed top side", 
			ForeignLanguageWord = "this is changed bottom side"
		};
		_ = flipper.ShowBottomSide();
	}
}