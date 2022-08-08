using System.Diagnostics;

namespace WordListsUI.WordDataPages.SideMenu.MenuField;

public partial class SideMenuField : Grid
{
	public SideMenuField()
	{
		BindingContext = this;
		InitializeComponent();
	}

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
    public double IconWidth
    {
        get => (double)GetValue(IconWidthProperty);
        set => SetValue(IconWidthProperty, value);
    }
    public string IconSource
    {
        get => (string)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }
    public Color SelectedColor
    {
        get => (Color)GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    public string TargetUIRoute { get; set; } = null;


    private async void Field_Tapped(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TargetUIRoute))
        {
            Debug.WriteLine($"{nameof(SideMenuField)} can't go to empty route");
            return;
        }
        await Shell.Current.GoToAsync(TargetUIRoute);
    }





    private static void IsSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SideMenuField field)
        {
            if ((bool)newValue)
            {
                field.mainBorder.BackgroundColor = field.SelectedColor;
                return;
            }
            field.mainBorder.BackgroundColor = field.BackgroundColor;
        }
    }

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected), typeof(bool), typeof(SideMenuField),
        defaultValue: false, propertyChanged: IsSelectedPropertyChanged);

    public static readonly BindableProperty IconWidthProperty = BindableProperty.Create(
        nameof(IconWidth), typeof(double), typeof(SideMenuField), defaultValue: 35d);

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IconSource), typeof(string), typeof(SideMenuField), defaultValue: "dotnet_bot.png");

    public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(
        nameof(SelectedColor), typeof(Color), typeof(SideMenuField), defaultValue: Color.FromArgb("#282E33"));
    
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(SideMenuField), defaultValue:"My text");

  
   
}