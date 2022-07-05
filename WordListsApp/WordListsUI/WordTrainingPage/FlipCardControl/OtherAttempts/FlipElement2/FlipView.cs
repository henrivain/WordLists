using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
namespace WordListsUI.WordTrainingPage.FlipElement2;

public class FlipView : ContentView
{
    private readonly RelativeLayout _contentHolder;

    public FlipView()
    {
        _contentHolder = new();
        Content = _contentHolder;
    }

    public static readonly BindableProperty FrontViewProperty =
		BindableProperty.Create(
            nameof(FrontView),
            typeof(View),
            typeof(FlipView),
            null,
            BindingMode.Default,
            null,
            FrontViewPropertyChanged
            );

    private static void FrontViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is null) return;

        ((FlipView)bindable)
            ._contentHolder
            .Children
            .Add(((FlipView)bindable).FrontView,
            Constraint.Constant(0),
            Constraint.Constant(0),
            Constraint.RelativeToParent((parent) => parent.Width),
            Constraint.RelativeToParent((parent) => parent.Height)
            );
    }

    public View FrontView
    {
        get => (View)GetValue(FrontViewProperty);
        set => SetValue(FrontViewProperty, value);
    }


    public static readonly BindableProperty BackViewProperty =
        BindableProperty.Create(
            nameof(BackView),
            typeof(View),
            typeof(FlipView),
            null,
            BindingMode.Default,
            null,
            BackViewPropertyChanged
            );

    private static void BackViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is null) return;

        ((FlipView)bindable)
            ._contentHolder
            .Children
            .Add(((FlipView)bindable).BackView,
            Constraint.Constant(0),
            Constraint.Constant(0),
            Constraint.RelativeToParent((parent) => parent.Width),
            Constraint.RelativeToParent((parent) => parent.Height)
            );

        ((FlipView)bindable).BackView.IsVisible = false;
    }

    public View BackView
    {
        get => (View)GetValue(BackViewProperty);
        set => SetValue(BackViewProperty, value);
    }




    public static readonly BindableProperty IsFlippedProperty =
        BindableProperty.Create(
            nameof(IsFlipped),
            typeof(bool),
            typeof(FlipView),
            false,
            BindingMode.Default,
            null,
            IsFlippedPropertyChanged
            );

    public bool IsFlipped
    {
        get => (bool)GetValue(IsFlippedProperty);
        set => SetValue(IsFlippedProperty, value);
    }

    private static void IsFlippedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((bool)newValue)
        {
            ((FlipView)bindable).FlipFromFrontToBack();
            return;
        }
        ((FlipView)bindable).FlipFromBackToFront();
    }









    private async void FlipFromFrontToBack()
    {
        await FrontToBackRotate();
        FrontView.IsVisible = false;
        BackView.IsVisible = true;
        await BackToFrontRotate();
    }





    private async void FlipFromBackToFront()
    {
        await FrontToBackRotate();
        FrontView.IsVisible = true;
        BackView.IsVisible = false;
        await BackToFrontRotate();
    }


    
    private async Task<bool> FrontToBackRotate()
    {
        Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(this);
        RotationY = 360;
        await Microsoft.Maui.Controls.ViewExtensions.RotateYTo(this, 270, 500, Easing.Linear);
        return true;
    }

    private async Task<bool> BackToFrontRotate()
    {
        Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(this);
        RotationY = 90;
        await Microsoft.Maui.Controls.ViewExtensions.RotateYTo(this, 0, 500, Easing.Linear);
        return true;
    }
}