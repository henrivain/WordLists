namespace WordListsUI.Components.SideMenu;
public partial class SideMenu : Grid
{
	public SideMenu()
	{
        BindingContext = this;
        InitializeComponent();
	}

    public void Collapse(object sender, EventArgs e)
    {
        IsCollapsed = true;
    }
    public void Open(object sender, EventArgs e)
    {
        IsCollapsed = false;
    }
    
    public string HamburgerImageSource { set => Resources["HamburgerImageSource"] = value; }

    public bool IsCollapsed
    {
        get => _isCollapsed;
        set
        {
            _isCollapsed = value;
            ChangeWidth(this, value ? MenuWidth_Collapsed : MenuWidth_Opened);
        }

    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(SideMenu), defaultValue: "Title");

    public Color TopSideColor
    {
        get => (Color)GetValue(TopSideColorProperty);
        set => SetValue(TopSideColorProperty, value);
    }
    public static readonly BindableProperty TopSideColorProperty = BindableProperty.Create(
        nameof(TopSideColor), typeof(Color), typeof(SideMenu), defaultValue: Color.FromArgb("#1A1E21"));

    public double MenuWidth_Opened
    {
        get => (double)GetValue(MenuWidth_OpenedProperty);
        set => SetValue(MenuWidth_OpenedProperty, value);
    }
    public static readonly BindableProperty MenuWidth_OpenedProperty = BindableProperty.Create(
        nameof(MenuWidth_Opened), typeof(double), typeof(SideMenu), 
        defaultValue: 200d, propertyChanged: MenuWidth_OpenedPropertyChanged);

    public double MenuWidth_Collapsed
    {
        get => (double)GetValue(MenuWidth_CollapsedProperty);
        set => SetValue(MenuWidth_CollapsedProperty, value);
    }
    public static readonly BindableProperty MenuWidth_CollapsedProperty = BindableProperty.Create(
        nameof(MenuWidth_Collapsed), typeof(double), typeof(SideMenu), 
        defaultValue:35d, propertyChanged: MenuWidth_CollapsedPropertyChanged);

    public View MenuItems
    {
        get => (View)GetValue(MenuItemsProperty);
        set => SetValue(MenuItemsProperty, value);
    }
    public static readonly BindableProperty MenuItemsProperty = BindableProperty.Create(
        nameof(MenuItems), typeof(View), typeof(SideMenu));

    bool _isCollapsed = false;

    private void ContentPresenter_SetMenuItemsBinding(object sender, EventArgs e)
    {
        if (sender is ContentPresenter presenter)
        {
            presenter.SetBinding(ContentPresenter.ContentProperty, new Binding(nameof(MenuItems)));
        }
    }
    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        IsCollapsed = !IsCollapsed;
    }
    private static void ChangeWidth(View view, double width)
    {
        Animation animation = new(o =>
        { view.WidthRequest = o; }, view.Width, width, Easing.CubicIn);
        animation.Commit(view, "widthAnimation", 16, 250);
    }
    private static void MenuWidth_CollapsedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SideMenu menu && menu.IsCollapsed)
        {
            menu.IsCollapsed = true;
        }
    }
    private static void MenuWidth_OpenedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SideMenu menu && menu.IsCollapsed is false)
        {
            menu.IsCollapsed = false;
        }
    }

}