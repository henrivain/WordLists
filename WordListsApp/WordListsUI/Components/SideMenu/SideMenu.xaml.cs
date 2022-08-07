namespace WordListsUI.Components.SideMenu;

public partial class SideMenu : ContentView
{
	public SideMenu()
	{
		InitializeComponent();
        BindingContext = this;
	}

    private bool IsHidden { get; set; } = false;

    public View MenuItems
    {
        get => (View)GetValue(MenuItemsProperty);
        set
        {
            SetValue(MenuItemsProperty, value);
        }
    }

    public static readonly BindableProperty MenuItemsProperty = BindableProperty.Create(
    nameof(MenuItems), typeof(View), typeof(SideMenu));

    public void Collapse(object sender, EventArgs e)
    {
        if (IsHidden is false) ChangeMenuWidth();
    }
    public void Open(object sender, EventArgs e)
    {
        if (IsHidden) ChangeMenuWidth();
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        ChangeMenuWidth();
    }
    private void ChangeMenuWidth()
    {
        ChangeWidth(sideMenu, IsHidden ?
            (short)Resources["MaxMenuWidth"]
            : (short)Resources["MinMenuWidth"]);
        IsHidden = !IsHidden;
    }
    static void ChangeWidth(View view, double width)
    {
        Animation animation = new(o =>
        { view.WidthRequest = o; }, view.Width, width, Easing.CubicIn);
        animation.Commit(view, "widthAnimation", 16, 250);
    }


    public string Title{ set => Resources["Title"] = value; }
    public Color MenuColor { set => Resources["MenuColor"] = value; }
    public Color TopSideColor { set => Resources["TopSideColor"] = value; }
    public string HamburgerImageSource { set => Resources["HamburgerImageSource"] = value; }

   
}