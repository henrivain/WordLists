using WordDataAccessLibrary.ExportServices;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WordDataTests")]
namespace WordDataAccessLibrary.Extensions;
internal static class WordCollectionExtensions
{
    internal static List<IExportWordCollection> ToIJsonCollection(this List<WordCollection> collections)
    {
        return collections
            .Select(c => new ExportWordCollection().FromWordCollection(c))
            .Where(ec => ec is not null)
            .ToList();
    }

    
    internal static List<WordCollection> ResetLearnStates(this List<WordCollection> collections)
    {
        if (collections is null) return new();

        return collections.Select(
            x => {
                x.WordPairs = x.WordPairs.Select(
                x => {
                    x.LearnState = WordLearnState.NotSet;
                    return x;
                })
                .ToList();
                return x;
            })
            .ToList();
    }
}
