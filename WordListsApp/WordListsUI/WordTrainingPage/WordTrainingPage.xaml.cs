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



 
	private void ShowDefaultSideWithoutAnimation()
	{
        if (ShowNativeWordByDefault)
        {
            flipper.ShowTopSideWithoutAnimation();
			return;
        }
		flipper.ShowBottomSideWithoutAnimation();
    }

	private async void Button_LastCard(object sender, EventArgs e)
	{
        SlideAnimation animation = new(flipper);
        await animation.ToMaxRight();
        ShowDefaultSideWithoutAnimation();
        await animation.FromMaxLeftToMiddle();
    }

	private async void Button_NextCard(object sender, EventArgs e)
	{
        SlideAnimation animation = new(flipper);
        await animation.ToMaxLeft();
        ShowDefaultSideWithoutAnimation();
        await animation.FromMaxRightToMiddle();
    }

   


    private void ParentGrid_Loaded(object sender, EventArgs e)
    {
#if WINDOWS
        parentGrid.MaximumWidthRequest = 500;
#endif
    }
}