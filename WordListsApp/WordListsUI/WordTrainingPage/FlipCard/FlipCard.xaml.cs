namespace WordListsUI.WordTrainingPage.FlipCard;

public partial class FlipCard : ContentView
{
	public FlipCard()
	{
		InitializeComponent();
	}

    //private readonly RelativeLayout

    //public View TopSide
    //{
    //    get => (View)GetValue(TopViewProperty);
    //    set => SetValue(TopViewProperty, value);
    //}

    


    public uint FlipSpeed { private get; set; } = 250;

    private bool TopSideVisible { get; set; } = true;

    private double CurrentRotation = 0;

    private async void Flip(object sender, EventArgs e)
    {
        CurrentRotation += 60;
        await baseGrid.RotateXTo(CurrentRotation, FlipSpeed, Easing.Linear);

        return;
        TopSideVisible = !TopSideVisible;
        await baseGrid.RotateXTo(90, FlipSpeed, Easing.Linear);
        top.IsVisible = TopSideVisible;
        bottom.IsVisible = !TopSideVisible;
        baseGrid.RotationX = -90;
        await baseGrid.RotateXTo(0, FlipSpeed, Easing.Linear);
    }

   
}