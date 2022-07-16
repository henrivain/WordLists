using Microsoft.Maui.Controls;
using WordListsUI.WordTrainingPage.FlipCardControl;
using WordListsViewModels;

namespace WordListsUI.WordTrainingPage;

public partial class WordTrainingPage : ContentPage
{
	public WordTrainingPage(IWordTrainingViewModel model)
	{
        BindingContext = model;
		InitializeComponent();
	}

    public IWordTrainingViewModel Model => (IWordTrainingViewModel)BindingContext;

	public bool ShowNativeWordByDefault { get; set; } = true;



 
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

   



}