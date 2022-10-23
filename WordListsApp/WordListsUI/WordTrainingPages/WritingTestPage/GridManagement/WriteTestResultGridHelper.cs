namespace WordListsUI.WordTrainingPages.WritingTestPage.GridManagement;
internal class WriteTestResultGridHelper : WordTrainingPageGridHelper
{
    public WriteTestResultGridHelper(Grid baseGrid, View movingView) : base(baseGrid, movingView) { }

    protected new readonly Dictionary<GridState, Thickness> StateMargins = new()
    {
        [GridState.ExtraBig] = new Thickness(30, 10, 0, 0),
        [GridState.Big] = new Thickness(30, 10, 0, 0),
        [GridState.Normal] = new Thickness(0, 20, 0, 20),
        [GridState.Small] = new Thickness(0, 20, 0, 20)
    };

    protected new readonly Dictionary<GridState, ColumnDefinitionCollection> StateColumnDefinitions = new()
    {
        [GridState.ExtraBig] = new(StarColumn(1), StarColumn(2), StarColumn(1)),
        [GridState.Big] = new(StarColumn(1), StarColumn(2), StarColumn(1)),
        [GridState.Normal] = new(StarColumn(1), StarColumn(5), StarColumn(1)),
        [GridState.Small] = new(StarColumn(1), StarColumn(10), StarColumn(1)),
    };

    internal override bool ReSize()
    {
        if (ChooseState()) return true;
        SetColumnsAndRows();
        MovingView.Margin = StateMargins[State];
        return false;
    }
}
