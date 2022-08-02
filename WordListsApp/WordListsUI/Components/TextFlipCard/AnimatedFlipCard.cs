namespace WordListsUI.Components.TextFlipCard;

/// <summary>
/// Implements animation tap action on top of FlipCard
/// </summary>
public class AnimatedFlipCard : FlipCard
{
    public AnimatedFlipCard() : base() { }
    public AnimatedFlipCard(string topText, string bottomText) : base(topText, bottomText) { }

    public uint FlipAnimationSpeed { get; set; } = 175;
    protected override async void Tapped(object sender, EventArgs e)
    {
        ShowingTopSide = !ShowingTopSide;
        if (ShowingTopSide)
        {
            await FlipUp();
            return;
        }
        await FlipDown();
    }

    private async Task FlipUp()
    {
        await this.RotateXTo(90, FlipAnimationSpeed, Easing.Linear);
        UpdateText();
        RotationX = -90;
        await this.RotateXTo(0, FlipAnimationSpeed, Easing.Linear);
    }
    private async Task FlipDown()
    {
        await this.RotateXTo(-90, FlipAnimationSpeed, Easing.Linear);
        UpdateText();
        RotationX = 90;
        await this.RotateXTo(0, FlipAnimationSpeed, Easing.Linear);
    }

    protected override async Task ShowTopSide()
    {
        if (ShowingTopSide) return;
        ShowingTopSide = true;
        await FlipUp();
    }
    protected override async Task ShowBottomSide()
    {
        if (ShowingTopSide is false) return;
        ShowingTopSide = false;
        await FlipDown();
    }

    public void ShowTopSideNoAnimation()
    {
        ShowingTopSide = true;
        UpdateText();
    }
    public void ShowBottomSideNoAnimation()
    {
        ShowingTopSide = false;
        UpdateText();
    }
}
