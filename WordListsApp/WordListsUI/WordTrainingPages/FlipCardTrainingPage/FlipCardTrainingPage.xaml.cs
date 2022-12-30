using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordListsUI.Components.TextFlipCard;

namespace WordListsUI.WordTrainingPages.FlipCardTrainingPage;

[QueryProperty(nameof(StartCollection), nameof(StartCollection))]
[QueryProperty(nameof(StartWordCollection), nameof(StartWordCollection))]
public partial class FlipCardTrainingPage : ContentPage
{
    public FlipCardTrainingPage(IWordTrainingViewModel model)
	{
        BindingContext = model;
		InitializeComponent();
        _animator = new(flipper);
        Model.CollectionUpdated += Model_CollectionUpdatedEvent;
    }

    public WordCollection StartCollection 
    { 
        set
        {
            if (value is not null) Model.StartNew(value);
        } 
    }

    private async void Model_CollectionUpdatedEvent(object sender, DataBaseActionArgs e)
    {
        await DisplayAlert("Päivitetty!", $"Sanasto päivitettiin säilytykseen id:llä {e.RefIdString}", "OK");
    }

    public int StartWordCollection { set => StartNewCollectionById(value); }

    public IWordTrainingViewModel Model => (IWordTrainingViewModel)BindingContext;

	public bool ShowNativeWordByDefault { get; set; } = true;


    readonly SlideAnimation _animator;

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
        await _animator.ToMaxRight();
        Model.Previous();
        ShowDefaultSideWithoutAnimation();
        await _animator.FromMaxLeftToMiddle();
    }

	private async void Button_NextCard(object sender, EventArgs e)
	{
        await _animator.ToMaxLeft();
        Model.Next();
        ShowDefaultSideWithoutAnimation();
        await _animator.FromMaxRightToMiddle();
    }

    private void FlipperGrid_SizeChanged(object sender, EventArgs e)
    {
        #if WINDOWS
        if (sender is Grid grid)
        {
            WordTrainingPages.FlipCardTrainingPage.Helpers.FlipperResizer.Resize(grid, Width);
        }
        #endif
    }

    /// <summary>
    /// This method is called when StartWordCollection is initialized
    /// </summary>
    /// <param name="id"></param>
    private async void StartNewCollectionById(int id)
    {
        await Model.StartNewAsync(id);
    }
}