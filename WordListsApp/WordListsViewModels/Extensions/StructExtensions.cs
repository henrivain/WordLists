namespace WordListsViewModels.Extensions;
internal static class StructExtensions
{
    public static T FirstOrGivenDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T defaultValue) 
        where T : struct
    {
        foreach (var item in source)
        {
            if (predicate(item))
            {
                return item;
            }
        }
        return defaultValue;
    }
}
