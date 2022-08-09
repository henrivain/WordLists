namespace WordListsMauiHelpers.PageRouting;

public static class PageRoutes
{
    public static string GetRoute(Route route)
    {
        return route switch
        {
            Route.Training => "Training",
            Route.WordHandling => "WordHandling",
            Route.Backup => "Backup",
            Route.LifeTime => "LifeTime",
            _ => throw new NotImplementedException()
        };
    }
}
