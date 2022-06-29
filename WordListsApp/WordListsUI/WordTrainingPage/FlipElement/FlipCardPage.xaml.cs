namespace WordListsUI.WordTrainingPage.FlipElement;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FlipCardPage : ContentView
{
	public FlipCardPage()
	{
		InitializeComponent();
	}

	private double Rotate = 0;

	private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		if (card is null) return;

		if (card.FrontSide.IsVisible)
		{
			Rotate = -180;
			await card.RotateYTo(Rotate, 250);
			//card.FrontSide.IsVisible = false;
			//card.BackSide.IsVisible = true;
			//card.BackSide.RotationY = -180;
			card.Content = card.BackSide;
			return;
		}
		Rotate += 180;
		await card.RotateYTo(Rotate, 250);
		//card.BackSide.IsVisible = false;
		//card.FrontSide.IsVisible = true;
		//card.FrontSide.RotationY = -360;
		//card.Content = card.FrontSide;



		//if (card.FrontSide.IsVisible)
		//{
		//    Rotate += -180;
		//    await card.RotateYTo(Rotate, 250);
		//    card.FrontSide.IsVisible = false;
		//    card.BackSide.IsVisible = true;
		//    card.BackSide.RotationY = -180;
		//    card.Content = card.BackSide;
		//    return;
		//}
		//Rotate += 180;
		//await card.RotateYTo(Rotate, 250);
		//card.BackSide.IsVisible = false;
		//card.FrontSide.IsVisible = true;
		//card.FrontSide.RotationY = -360;
		//card.Content = card.FrontSide;
	}
}