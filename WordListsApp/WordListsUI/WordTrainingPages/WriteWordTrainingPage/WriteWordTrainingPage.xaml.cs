using System.Diagnostics;


namespace WordListsUI.WordTrainingPages.WriteWordTrainingPage;

public partial class WriteWordTrainingPage : ContentPage
{
	readonly GridHelper _gridHelper = new();

    public WriteWordTrainingPage(IWriteWordViewModel model)
	{
		BindingContext = model;
        InitializeComponent();
	}

	public IWriteWordViewModel Model => (IWriteWordViewModel)BindingContext;

	private void Grid_SizeChanged(object sender, EventArgs e)
	{
		if (sender is Grid grid)
		{
			_gridHelper.ReSize(grid);
		}
    }

	private void ChildGrid_SizeChanged(object sender, EventArgs e)
	{
		if (sender is Grid grid) 
		{
            switch (_gridHelper.State)
			{
				case GridHelper.GridState.ExtraBig:
					grid.HorizontalOptions = LayoutOptions.Start;
					grid.Margin = new Thickness(30, 40);
					break;

                case GridHelper.GridState.Big:
                    grid.HorizontalOptions = LayoutOptions.Start;
					grid.Margin = new Thickness(30, 40);
                    break;

				case GridHelper.GridState.Normal:
                    grid.HorizontalOptions = LayoutOptions.Center;
                    grid.Margin = new Thickness(0, 20, 0, 0);
					break;
			}
        }
		
    }
}