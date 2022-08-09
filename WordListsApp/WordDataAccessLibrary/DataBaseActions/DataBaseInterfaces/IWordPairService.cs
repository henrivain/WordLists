using System.Linq.Expressions;

namespace WordDataAccessLibrary.DataBaseActions.Interfaces;
public interface IWordPairService
{
    Task<List<WordPair>> GetAll();
    Task<List<WordPair>> GetByOwnerId(int ownerId);
    Task<List<WordPair>> GetByOwner(WordCollectionOwner owner);
    Task<List<WordPair>> GetByIdAndLearnState(int ownerId, WordLearnState learnState);
    Task UpdatePairsAsync(WordCollection collection);
    Task InsertPairsAsync(WordCollection collection);
    Task<int> CountItems();
    Task<int> CountItemsMatching(Expression<Func<WordPair, bool>> expression);
}
