namespace WordListsUI.WordTrainingPage.FlipCardControl;

public partial class FlipCard : ContentView
{
	public FlipCard()
	{
		InitializeComponent();
        UpdateText();
	}
    public FlipCard(string topText, string bottomText) : this()
    {
        TopSideText = topText;
        BottomSideText = bottomText;
    }
    protected bool ShowingTopSide { get; set; } = true;

    #region TextHandling

    protected virtual Task ShowTopSide()
    {
        ShowingTopSide = true;
        UpdateText();
        return Task.CompletedTask;
    }
    protected virtual Task ShowBottomSide()
    {
        ShowingTopSide = false;
        UpdateText();
        return Task.CompletedTask;
    }
    protected void UpdateText() => testGridText.Text = ShowingTopSide ? TopSideText : BottomSideText;
    public string TopSideText
    {
        get => (string)GetValue(TopTextProperty);
        set
        {
            SetValue(TopTextProperty, value);
            UpdateText();
        }
    }
    public string BottomSideText
    {
        get => (string)GetValue(BottomTextProperty);
        set
        {
            SetValue(BottomTextProperty, value);
            UpdateText();
        }
    }

    public static readonly BindableProperty TopTextProperty =
        BindableProperty.Create(nameof(TopSideText), typeof(string), typeof(FlipCard), "my top text");

    public static readonly BindableProperty BottomTextProperty =
        BindableProperty.Create(nameof(BottomSideText), typeof(string), typeof(FlipCard), "my bottom text");
    #endregion

    protected virtual void Tapped(object sender, EventArgs e) 
    {
        ShowingTopSide = !ShowingTopSide;
        UpdateText();
    }
}