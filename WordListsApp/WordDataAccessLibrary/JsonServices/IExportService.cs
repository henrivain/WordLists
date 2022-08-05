namespace WordDataAccessLibrary.JsonServices;

public interface IExportService
{
    Task<JsonActionArgs> ExportByOwners(List<WordCollectionOwner> owners, string path);
    Task<JsonActionArgs> Export(List<IExportWordCollection> exportCollections, string path);
}