using WordListsViewModels.Helpers;

namespace WordListsViewModels.Extensions;
internal static class ListExtensions
{
    internal static IEnumerable<WordCollectionOwner> SortByName(this IEnumerable<WordCollectionOwner> owners)
    {
        return owners.OrderBy(x => x.Name);
    }

    internal static IEnumerable<WordCollectionInfo> SortByName(this IEnumerable<WordCollectionInfo> owners)
    {
        return owners.OrderBy(x => x.Owner.Name).ToList();
    }

    internal static List<WordCollectionInfo> GetOwners(this List<object> objects)
    {
        if (objects is null) return new();
        return objects.Where(x => x is WordCollectionInfo)
                      .Select(x => (WordCollectionInfo)x)
                      .ToList();
    }

    /// <summary>
    /// Flip list members with another. For example 1 with 2 and so on...
    /// <para/>See example below 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <example>
    /// 
    /// input: a, b, c, d, e
    /// result: b, a, d, c
    /// e doesn't have pair, so it will be eliminated
    /// 
    /// </example>
    /// <returns>List of T where pairs are flipped or empty list, if list length is smaller than 2</returns>
    internal static List<T> FlipPairs<T>(this List<T> values)
    {
        List<T> result = Enumerable.Empty<T>().ToList();

        // e doesn't have pair, so it will be eliminated
        // input: a, b, c, d, e
        // result: b, a, d, c

        for (int i = 0; i + 1 < values.Count; i += 2)
        {
            result.Add(values[i + 1]);
            result.Add(values[i]);
        }
        return result;
    }
}
