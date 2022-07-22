using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WordListsUI.WordTrainingPage.FlipCardControl;

/// <summary>
/// Implement card like view with two sides for text. Side is changed when tapped (No animation) 
/// </summary>
public partial class FlipCard : ContentView
{
	public FlipCard()
	{
		InitializeComponent();
        //BindingContext = new FlipCardViewModel();
        UpdateText();
	}
    public FlipCard(string topText, string bottomText) : this()
    {
        TopSideText = topText;
        BottomSideText = bottomText;
    }
    protected bool ShowingTopSide { get; set; } = true; 


    //public IFlipCardViewModel Model => (IFlipCardViewModel)BindingContext;

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

    protected virtual void UpdateText()
    {
        textField.Text = ShowingTopSide ? TopSideText : BottomSideText;
    }




    public virtual string TopSideText
    {
        get => (string)GetValue(TopTextProperty);
        set
        {
            SetValue(TopTextProperty, value);
            UpdateText();
        }
    }

    public virtual string BottomSideText
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


    protected Color CardColor { set => card.BackgroundColor = value; }

    protected virtual void Tapped(object sender, EventArgs e) 
    {
        ShowingTopSide = !ShowingTopSide;
        UpdateText();
    }
}