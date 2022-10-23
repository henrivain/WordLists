namespace WordListsUI.WordTrainingPages.FlipCardTrainingPage.Helpers;
internal static class FlipperResizer
{
    internal static void Resize(Grid sender, double viewHeight)
    {
        sender.ColumnDefinitions = viewHeight switch
        {
            > 1000 => new(new ColumnDefinition(GridLength.Star),
                         new ColumnDefinition(GridLength.Star),
                         new ColumnDefinition(GridLength.Star)
                         ),
            <= 1000 => new(new ColumnDefinition(),
                         new ColumnDefinition(330),
                         new ColumnDefinition()
                         ),

            _ => new(new ColumnDefinition(),
                         new ColumnDefinition(),
                         new ColumnDefinition()
                         ),
        };
    }
}
