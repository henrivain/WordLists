using System.Linq.Expressions;

namespace WordDataAccessLibrary.DataBaseActions.Interfaces;
public interface IWordPairService
{
    Task<List<WordPair>> GetAll();
    Task<List<WordPair>> GetByOwnerId(int ownerId);
    Task<List<WordPair>> GetByOwner(WordCollectionOwner owner);
    Task UpdatePairsAsync(WordCollection collection);
    Task InsertPairsAsync(WordCollection collection);
    Task<int> CountItems();
    Task<int> CountItemsMatching(Expression<Func<WordPair, bool>> expression);
    Task UpdatePairAsync(WordPair pair);

    /// <summary>
    /// Get element by its primary key, might return NULL
    /// </summary>
    /// <param name="key"></param>
    /// <returns>NULL if not found, else returns matching wordpair</returns>
    Task<WordPair> GetByPrimaryKey(int key);

    Task<List<WordPair>> GetByExpression(Expression<Func<WordPair, bool>> expression);
}
