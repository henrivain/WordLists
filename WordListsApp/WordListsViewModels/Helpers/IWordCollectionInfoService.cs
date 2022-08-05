namespace WordListsViewModels.Helpers;

public interface IWordCollectionInfoService
{
    Task<List<WordCollectionInfo>> GetAll();
}