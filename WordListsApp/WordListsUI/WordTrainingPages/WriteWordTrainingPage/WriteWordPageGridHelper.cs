

using System.Diagnostics;
using System.Net.Http.Headers;

namespace WordListsUI.WordTrainingPages.WriteWordTrainingPage;
internal class WriteWordPageGridHelper
{
    public WriteWordPageGridHelper(Grid baseGrid, VerticalStackLayout infoGrid)
    {
        BaseGrid = baseGrid;
        InfoGrid = infoGrid;
    }

    internal enum GridState
    {
        ExtraBig, Big, Normal, Small
    }

    internal GridState State { get; private set; } = GridState.Normal;
    Grid BaseGrid { get; }
    VerticalStackLayout InfoGrid { get; }

    internal static readonly double DisplayHeight = DeviceDisplay.Current.MainDisplayInfo.Height;
    internal static readonly double DisplayWidth = DeviceDisplay.Current.MainDisplayInfo.Width;

    private static ColumnDefinition StarColumn(int starLenght)
    {
        return new()
        {
            Width = new GridLength(starLenght, GridUnitType.Star)
        };
    }
    private static RowDefinition StarRow(int starLenght)
    {
        return new()
        {
            Height = new GridLength(starLenght, GridUnitType.Star)
        };
    }
    private static RowDefinition StartRow()
    {
        return new() { Height = new(1, GridUnitType.Auto) };
    }


    /// <summary>
    /// Choose and set state to match current state of ui
    /// </summary>
    /// <returns>true if new state is same as old, else false</returns>
    private bool ChooseState()
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

    internal void ReSize()
    {
        if (ChooseState()) return;
        switch (State)
        {
            case GridState.ExtraBig:
                BaseGrid.ColumnDefinitions = new(StarColumn(1), StarColumn(2), StarColumn(1));
                BaseGrid.RowDefinitions = new(StarRow(1));
                InfoGrid.Margin = new Thickness(30, 40, 0, 0);
                Grid.SetColumn(InfoGrid, 0);
                break;

            case GridState.Big:
                BaseGrid.ColumnDefinitions = new(StarColumn(1), StarColumn(3), StarColumn(1));
                BaseGrid.RowDefinitions = new(StarRow(1));
                InfoGrid.Margin = new Thickness(30, 40, 0, 0);
                Grid.SetColumn(InfoGrid, 0);
                break;

            case GridState.Normal:
                BaseGrid.ColumnDefinitions = new(StarColumn(1), StarColumn(5), StarColumn(1));
                BaseGrid.RowDefinitions = new(StarRow(1), StartRow());
                InfoGrid.Margin = new Thickness(0, 20, 0, 0);
                Grid.SetColumn(InfoGrid, 1);
                break;

            case GridState.Small:
                BaseGrid.ColumnDefinitions = new(StarColumn(1), StarColumn(10), StarColumn(1));
                BaseGrid.RowDefinitions = new(StarRow(1), StartRow());
                InfoGrid.Margin = new Thickness(0, 20, 0, 0);
                Grid.SetColumn(InfoGrid, 1);
                break;
        }
    }
}
