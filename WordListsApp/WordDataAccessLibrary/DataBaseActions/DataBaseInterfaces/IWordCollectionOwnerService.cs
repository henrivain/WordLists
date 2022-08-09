namespace WordDataAccessLibrary.DataBaseActions.Interfaces;
public interface IWordCollectionOwnerService
{
    Task<List<WordCollectionOwner>> GetAll();
    Task<List<WordCollectionOwner>> GetByLanguage(string languageHeaders);
    Task<List<WordCollectionOwner>> GetByName(string name);
    Task<WordCollectionOwner> GetById(int id);
    Task<int> CountItems();
}
