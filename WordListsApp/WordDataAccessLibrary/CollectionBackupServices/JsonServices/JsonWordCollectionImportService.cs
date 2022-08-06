using Newtonsoft.Json;

namespace WordDataAccessLibrary.CollectionBackupServices.JsonServices;
public class JsonWordCollectionImportService : ICollectionImportService
{

	public async Task<(ImportActionResult, IEnumerable<IExportWordCollection>)> Import(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || Path.GetExtension(path) is not ".json")
		{
			return (new ImportActionResult(BackupAction.Configure)
			{
				MoreInfo = "Bad path (empty or no '.json')",
				UsedPath = path,
				Success = false
			}, null);
		}

		ImportActionResult result;
		(result, string jsonData) = await ReadJsonString(path);

		if (result.Success is false) return (result, null);

		return JsonToExportCollections(jsonData);
	}

    private static (ImportActionResult, IEnumerable<IExportWordCollection>) JsonToExportCollections(string jsonData)
	{
		ImportActionResult result = new(BackupAction.ParseData)
		{
			Success = false
		};
		JsonBackupStruct? parsedData;

        try
		{
            parsedData = JsonConvert.DeserializeObject<JsonBackupStruct>(jsonData);
        }
		catch(Exception ex)
		{
            result.MoreInfo = $"Parsing json data to {nameof(JsonBackupStruct)} failed because of {ex}, {ex.Message}";
#if DEBUG
			throw;
#else
			return (result, null);
#endif
		}

		if (parsedData is null)
		{
			result.MoreInfo = $"{nameof(JsonBackupStruct)} parsed from json is null, " +
				$"given json (file) might be broken or data type is wrong";
			return (result, null);
		}
		
		if (parsedData?.Collections is null)
		{
			result.MoreInfo = $"Parsed data does not have any word collection in it";
			return (result, null);
		}
		result.Success = true;
        return (result, parsedData?.Collections);
	}

	private static async Task<(ImportActionResult, string)> ReadJsonString(string path)
	{
		ImportActionResult result = new(BackupAction.ReadFile)
		{
			Success = true,
			UsedPath = path
		};
        string jsonData = null;

        try
        {
			using StreamReader sr = new(path);
			jsonData = await sr.ReadToEndAsync();
            if (string.IsNullOrEmpty(jsonData))
            {
                jsonData = null;
				result.MoreInfo = "Opened json file was empty";
            }
        }
		catch (Exception ex)
		{
            result.MoreInfo = $"{nameof(JsonWordCollectionImportService)} " +
                $"Read file at {nameof(ImportActionResult.UsedPath)} failed because of {ex}, {ex.Message}";

#if DEBUG
            throw;
#endif
        }
		if (jsonData is null)
		{
			result.Success = false;
		}
        return (result, jsonData);
    }
}


