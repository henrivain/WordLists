using Microsoft.Maui.Controls;
using System.Diagnostics;
using WordDataAccessLibrary;
using WordListsUI.WordTrainingPage.FlipCardControl;
using WordListsUI.WordTrainingPage.Helpers;
using WordListsViewModels;


namespace WordListsUI.WordTrainingPage;

public partial class WordTrainingPage : ContentPage
{
    public WordTrainingPage(IWordTrainingViewModel model)
	{
        BindingContext = model;
		InitializeComponent();
        Animator = new(flipper);
	}

    public IWordTrainingViewModel Model => (IWordTrainingViewModel)BindingContext;

	public bool ShowNativeWordByDefault { get; set; } = true;


    readonly SlideAnimation Animator;

    private void ShowDefaultSideWithoutAnimation()
	{
        if (ShowNativeWordByDefault)
        {
            flipper.ShowTopSideNoAnimation();
			return;
        }
		flipper.ShowBottomSideNoAnimation();
    }

	private async void Button_LastCard(object sender, EventArgs e)
	{
        await Animator.ToMaxRight();
        Model.Previous();
        ShowDefaultSideWithoutAnimation();
        await Animator.FromMaxLeftToMiddle();
    }

	private async void Button_NextCard(object sender, EventArgs e)
	{
        await Animator.ToMaxLeft();
        Model.Next();
        ShowDefaultSideWithoutAnimation();
        await Animator.FromMaxRightToMiddle();
    }

    private void FlipperGrid_SizeChanged(object sender, EventArgs e)
    {
        #if WINDOWS
        if (sender is Grid grid)
        {
            FlipperResizer.Resize(grid, Width);
        }
        #endif
    }
}