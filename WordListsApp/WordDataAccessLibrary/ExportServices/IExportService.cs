namespace WordDataAccessLibrary.ExportServices;

public interface IExportService
{
    Task<ExportActionResult> ExportByCollectionOwners(List<WordCollectionOwner> owners, string path);
    Task<ExportActionResult> Export(List<IExportWordCollection> exportCollections, string path);
}