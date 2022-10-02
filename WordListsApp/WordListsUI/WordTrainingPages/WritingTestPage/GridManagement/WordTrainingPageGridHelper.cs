namespace WordListsUI.WordTrainingPages.WritingTestPage.GridManagement;
internal class WordTrainingPageGridHelper
{
    public WordTrainingPageGridHelper(Grid baseGrid, View movingView)
    {
        BaseGrid = baseGrid;
        MovingView = movingView;
    }

    internal enum GridState
    {
        ExtraBig, Big, Normal, Small
    }

    internal GridState State { get; private set; } = GridState.Normal;
    protected Grid BaseGrid { get; }
    protected View MovingView { get; }

    internal static readonly double DisplayHeight = DeviceDisplay.Current.MainDisplayInfo.Height;
    internal static readonly double DisplayWidth = DeviceDisplay.Current.MainDisplayInfo.Width;

    protected readonly Dictionary<GridState, Thickness> StateMargins = new()
    {
        [GridState.ExtraBig] = new Thickness(30, 30, 0, 0),
        [GridState.Big] = new Thickness(30, 30, 0, 0),
        [GridState.Normal] = new Thickness(0, 20, 0, 0),
        [GridState.Small] = new Thickness(0, 20, 0, 0)
    };

    protected readonly Dictionary<GridState, ColumnDefinitionCollection> StateColumnDefinitions = new()
    {
        [GridState.ExtraBig] = new(StarColumn(1), StarColumn(2), StarColumn(1)),
        [GridState.Big] = new(StarColumn(1), StarColumn(3), StarColumn(1)),
        [GridState.Normal] = new(StarColumn(1), StarColumn(5), StarColumn(1)),
        [GridState.Small] = new(StarColumn(1), StarColumn(10), StarColumn(1)),
    };

    protected readonly Dictionary<GridState, RowDefinitionCollection> StateRowDefinitions = new()
    {
        [GridState.ExtraBig] = new(StarRow(1)),
        [GridState.Big] = new(StarRow(1)),
        [GridState.Normal] = new(StarRow(1), AutoRow()),
        [GridState.Small] = new(StarRow(1), AutoRow()),
    };


    protected static ColumnDefinition StarColumn(double starLenght)
    {
        return new() { Width = new GridLength(starLenght, GridUnitType.Star) };
    }
    protected static RowDefinition StarRow(double starLenght)
    {
        return new() { Height = new GridLength(starLenght, GridUnitType.Star) };
    }
    protected static RowDefinition AutoRow()
    {
        return new() { Height = new(1, GridUnitType.Auto) };
    }


    /// <summary>
    /// Choose and set state to match current state of ui
    /// </summary>
    /// <returns>true if new state is same as old, else false</returns>
    protected virtual bool ChooseState()
    {
        GridState oldState = State;
        if (BaseGrid.Width > DisplayWidth / 2)
        {
            State = GridState.ExtraBig;
        }
        else if (BaseGrid.Width > DisplayWidth / 2.6)
        {
            State = GridState.Big;
        }
        else if (BaseGrid.Width > DisplayWidth / 3.6)
        {
            State = GridState.Normal;
        }
        else if (BaseGrid.Width < DisplayWidth / 3.6)
        {
            State = GridState.Small;
        }
        return oldState == State;
    }

    protected virtual void SetColumnsAndRows()
    {
        BaseGrid.ColumnDefinitions = StateColumnDefinitions[State];
        BaseGrid.RowDefinitions = StateRowDefinitions[State];
        Grid.SetColumn(MovingView, State switch
        {
            GridState.ExtraBig => 0,
            GridState.Big => 0,
            GridState.Normal => 1,
            GridState.Small => 1,
            _ => throw new NotImplementedException()
        });
    }




    /// <summary>
    /// Resize grid to fit current window size
    /// </summary>
    /// <returns>true if old state remains, returns false if changes are needed </returns>
    internal virtual bool ReSize()
    {
        if (ChooseState()) return true;
        MovingView.MaximumWidthRequest = 600;
        SetColumnsAndRows();
        MovingView.Margin = StateMargins[State];
        return false;
    }
}
