namespace WordListsMauiHelpers.Extensions;

public static class ListExtensions
{
    private static readonly Random _random = new();

    public static List<T> Shuffle<T>(this List<T> list)
    {
        int i = list.Count;
        while (i > 1)
        {
            i--;
            int j = _random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }

}
