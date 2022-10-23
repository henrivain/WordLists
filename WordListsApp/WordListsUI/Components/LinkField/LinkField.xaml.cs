using CommunityToolkit.Mvvm.ComponentModel;

namespace WordListsUI.Components.LinkField;

public partial class LinkField : Border
{
	public LinkField()
	{
		InitializeComponent();
		BindingContext = this;
	}

	private void TapGestureRecognizer_Tapped(object sender, EventArgs e) => Tapped?.Invoke(this, EventArgs.Empty);

	public event EventHandler Tapped;

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

    public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(LinkField), "asdsaasd", BindingMode.TwoWay);

}