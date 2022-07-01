using Microsoft.Maui.Controls;
using WordListsUI.WordTrainingPage.FlipCardControl;

namespace WordListsUI.WordTrainingPage;

public partial class WordTrainingPage : ContentPage
{
	public WordTrainingPage()
	{
		InitializeComponent();
	}

	private bool ShowNativeWordByDefault = true;



    private async void Button_Clicked(object sender, EventArgs e)
	{
		await flipper.TranslateTo(-50, 0, 125, Easing.Linear);
		ShowDefaultSideWithoutAnimation();
		flipper.TranslationX = 50;
		await flipper.TranslateTo(0, 0, 125, Easing.Linear);

		//SlideOutInAnimation animation = new(flipper);
		//animation.Run();

	}

	private void ShowDefaultSideWithoutAnimation()
	{
        if (ShowNativeWordByDefault)
        {
            flipper.ShowTopSideWithoutAnimation();
			return;
        }
		flipper.ShowBottomSideWithoutAnimation();
    }
}