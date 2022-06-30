using WordDataAccessLibrary;

namespace WordListsUI.WordTrainingPage.FlipCardControl;

public partial class FlipCard : ContentView
{
	public FlipCard()
	{
		InitializeComponent();
        SetSideText();
	}

    public FlipCard(string topText, string bottomText) : this()
    {
        TopSideText = topText;
        BottomSideText = bottomText;
    }

    public FlipCard(WordPair pair, bool showNativeWord)
    {
        WordPair = pair;
        ShowingTopSide = showNativeWord;
        SetSideText();
    }


    public static readonly BindableProperty TopTextProperty =
        BindableProperty.Create(
            nameof(TopSideText),
            typeof(string),
            typeof(FlipCard),
            "my top text",
            BindingMode.Default
            );

    public static readonly BindableProperty BottomTextProperty =
        BindableProperty.Create(
            nameof(BottomSideText),
            typeof(string),
            typeof(FlipCard),
            "my bottom text",
            BindingMode.Default
            );

    WordPair _wordPair;
    public WordPair WordPair 
    {
        get => _wordPair;
        set
        {
            _wordPair = value;
            ResetSideTextsFromWordPair(value);
        }
    }

    private void ResetSideTextsFromWordPair(WordPair pair)
    {
        if (pair is null) return;
        TopSideText = pair.NativeLanguageWord;
        BottomSideText = pair.ForeignLanguageWord;
    }
    private bool ShowingTopSide { get; set; } = true;
    public uint AnimationSpeed { get; set; } = 175;
    public string TopSideText
    {
        get => (string)GetValue(TopTextProperty);
        set
        {
            SetValue(TopTextProperty, value);
            SetSideText();
        }
    }
    public string BottomSideText
    {
        get => (string)GetValue(BottomTextProperty);
        set
        {
            SetValue(BottomTextProperty, value);
            SetSideText();
        }
    }
    public async Task ShowTopSide()
    {
        if (ShowingTopSide) return;
        ShowingTopSide = true;
        await FlipUp();
    }
    public async Task ShowBottomSide()
    {
        if (ShowingTopSide is false) return;
        ShowingTopSide = false;
        await FlipDown();
    }


    private void SetSideText()
    {
        testGridText.Text = ShowingTopSide ? TopSideText : BottomSideText;
    }
    private async void Flip(object sender, EventArgs e)
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
        await testGrid.RotateXTo(90, AnimationSpeed, Easing.Linear);
        SetSideText();
        testGrid.RotationX = -90;
        await testGrid.RotateXTo(0, AnimationSpeed, Easing.Linear);
    }
    private async Task FlipDown()
    {
        await testGrid.RotateXTo(-90, AnimationSpeed, Easing.Linear);
        SetSideText();
        testGrid.RotationX = 90;
        await testGrid.RotateXTo(0, AnimationSpeed, Easing.Linear);
    }


}