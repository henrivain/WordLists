using Microsoft.Maui.Controls;

namespace WordListsUI.WordTrainingPage;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class WordTrainingPage : ContentPage
{
	public WordTrainingPage()
	{
		InitializeComponent();
		SetSideProps();
	}


	private bool TopSideVisible { get; set; } = true;

	public uint AnimationSpeed { get; set; } = 175;


	public struct SideInfo
	{
		public SideInfo() { }
		public SideInfo(string text) => Text = text;
		public SideInfo(Color color) => Color = color;
		public SideInfo(string text, Color color) : this(text) => Color = color;
		public string Text { get; set; } = string.Empty;
		public Color Color { get; set; } = Colors.Transparent;
	}

	

	public SideInfo TopSide { get; set; } = 
		new("my top text", Colors.AliceBlue);
	
	public SideInfo BottomSide { get; set; } = 
		new("my bottom text", Colors.AliceBlue);


	private void SetSideProps()
	{
        if (TopSideVisible)
		{
			testGridText.Text = TopSide.Text;
			testGrid.BackgroundColor = TopSide.Color;
			return;
		}
        testGridText.Text = BottomSide.Text;
        testGrid.BackgroundColor = BottomSide.Color;
    }

	private async void Flip(object sender, EventArgs e)
	{
        TopSideVisible = !TopSideVisible;
        if (TopSideVisible)
		{
			await FlipUp();
			return;
		}
        await FlipDown();

        //await testGrid.RotateXTo(90, 250, Easing.Linear);
        //SetSideProps();
        //testGrid.RotationX = -90;
        //await testGrid.RotateXTo(0, 250, Easing.Linear);
    }

    private async Task FlipUp()
	{
        await testGrid.RotateXTo(90, AnimationSpeed, Easing.Linear);
        SetSideProps();
        testGrid.RotationX = -90;
        await testGrid.RotateXTo(0, AnimationSpeed, Easing.Linear);
    }

	private async Task FlipDown()
	{
        await testGrid.RotateXTo(-90, AnimationSpeed, Easing.Linear);
        SetSideProps();
        testGrid.RotationX = 90;
        await testGrid.RotateXTo(0, AnimationSpeed, Easing.Linear);
    }
}