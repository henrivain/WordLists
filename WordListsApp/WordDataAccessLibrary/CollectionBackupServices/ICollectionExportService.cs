namespace WordDataAccessLibrary.CollectionBackupServices;

public interface ICollectionExportService
{
    Task<ExportActionResult> ExportByCollectionOwners(List<WordCollectionOwner> owners, string path);
    Task<ExportActionResult> ExportByCollectionOwners(List<WordCollectionOwner> owners, string path, bool removeUserData);
    Task<ExportActionResult> Export(List<IExportWordCollection> exportCollections, string path);
}