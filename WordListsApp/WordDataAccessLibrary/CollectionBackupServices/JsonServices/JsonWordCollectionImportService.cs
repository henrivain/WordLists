namespace WordDataAccessLibrary.CollectionBackupServices.JsonServices;
public class JsonWordCollectionImportService : ICollectionImportService
{
	public JsonWordCollectionImportService(ICollectionImportService importService)
	{
		ImportService = importService;
	}

	public ICollectionImportService ImportService { get; }
}


