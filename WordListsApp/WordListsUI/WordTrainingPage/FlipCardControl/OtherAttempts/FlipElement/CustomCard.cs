namespace WordListsUI.WordTrainingPage.FlipElement;

public class CustomCard : ContentView
{
    public static readonly BindableProperty BackSideProperty =
        BindableProperty.Create(
            nameof(BackSide),
            typeof(View),
            typeof(CustomCard),
            null,
            BindingMode.Default,
            null,
            null
            );


    public static readonly BindableProperty FrontSideProperty =
    BindableProperty.Create(
        nameof(FrontSide),
        typeof(View),
        typeof(CustomCard),
        null,
        BindingMode.Default,
        null,
        null,
        FrontSidePropertyChanged
        );




    public View BackSide
    {
        get => (View)GetValue(BackSideProperty); 
        set => SetValue(BackSideProperty, value); 
    }

    public View FrontSide
    {
        get => (View)GetValue(FrontSideProperty);
        set => SetValue(FrontSideProperty, value);
    }


    private static void FrontSidePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is null) return;
        if (bindable is CustomCard customCard)
        {
            if (customCard.FrontSide is null) return;
            if (customCard.FrontSide.IsVisible is false) return;
            customCard.Content = customCard.FrontSide;
        }
    }
}