namespace WordDataAccessLibrary.CollectionBackupServices;

public interface ICollectionImportService
{
    Task<(ImportActionResult, IEnumerable<IExportWordCollection>)> Import(string path);
}