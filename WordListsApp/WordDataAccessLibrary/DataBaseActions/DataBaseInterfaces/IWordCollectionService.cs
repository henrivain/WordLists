namespace WordDataAccessLibrary.DataBaseActions.Interfaces;
public interface IWordCollectionService
{
    /// <param name="collection"></param>
    /// <returns>database id of added collection</returns>
    Task<int> AddWordCollection(WordCollection collection);
    Task SaveProgression(WordCollection wordCollection);
    Task<List<WordCollection>> GetWordCollections();
    Task<WordCollection> GetWordCollection(int id);
    Task<List<WordCollection>> GetWordCollectionsById(int[] ids);
    Task<int> DeleteWordCollection(int OwnerId);

    /// <summary>
    /// WARNING!!!  DELETES ALL OBJECTS FROM DATABASE! execute by passing "true" as parameter, otherwise will not run
    /// </summary>
    /// <param name="verifyByTrue"></param>
    /// <returns>number of object deleted</returns>
    Task<int> DeleteAll(string verifyByTrue);
    Task<int> CountItems();
}
