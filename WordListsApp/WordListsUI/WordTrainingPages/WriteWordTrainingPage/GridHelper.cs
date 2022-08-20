

using System.Diagnostics;
using System.Net.Http.Headers;

namespace WordListsUI.WordTrainingPages.WriteWordTrainingPage;
internal class GridHelper
{
    internal enum GridState
    {
        ExtraBig, Big, Normal
    }

    internal GridState State { get; private set; } = GridState.Normal;

    internal static readonly double DisplayHeight = DeviceDisplay.Current.MainDisplayInfo.Height;
    internal static readonly double DisplayWidth = DeviceDisplay.Current.MainDisplayInfo.Width;

    private static ColumnDefinition GetColumn(int starLenght)
    {
        return new()
        {
            Width = new GridLength(starLenght, GridUnitType.Star)
        };
    }
    private static RowDefinition GetRow(int starLenght)
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

    internal void ReSize(Grid grid)
	{
        if (grid.Width > DisplayWidth / 2)
        {
            if (State == GridState.ExtraBig) return;
            grid.ColumnDefinitions = new(GetColumn(1), GetColumn(2), GetColumn(1));
            grid.RowDefinitions = new(GetRow(1));
            State = GridState.ExtraBig;
            return;
        }
        if (grid.Width > DisplayWidth / 2.6)
        {
            if (State == GridState.Big) return;
            grid.ColumnDefinitions = new(GetColumn(1), GetColumn(3), GetColumn(1));
            grid.RowDefinitions = new(GetRow(1));
            State = GridState.Big;
            return;
        }
        if (grid.Width < DisplayWidth / 2.6)
        {
            if (State == GridState.Normal) return;
            grid.ColumnDefinitions = new(GetColumn(1));
            grid.RowDefinitions = new(GetRow(1), StartRow());
            State = GridState.Normal;
            return;
        }
    }
}
