using WordDataAccessLibrary.ExportServices;
using WordDataAccessLibrary.ExportServices.JsonServices;

namespace WordDataTests.ServiceTests.ExportServiceTests;
public class JsonExportServiceTests
{
	public JsonExportServiceTests()
	{
		_service = new JsonWordCollectionExportService(null);

    }

	readonly JsonWordCollectionExportService _service;


	[Fact]
	public void ConvertDataToJson_WithNull_ShouldThrow_ArgumentException()
	{
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
		{
			JsonWordCollectionExportService.ConvertDataToJson(null);
		});
    }


    [Fact]
    public void ConvertDataToJson_WithEmptyList_ShouldThrow_ArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            JsonWordCollectionExportService.ConvertDataToJson(new());
        });
    }

    [Fact]
    public void ConvertDataToJson_ListLengthOne_ParseJson()
    {
        // Arrange
        List<IExportWordCollection> collections = new()
        {
            new ExportWordCollection()
            {
                Description = "this is description",
                Name = "name",
                LanguageHeaders = "fi-en",
                WordPairs = new ExportWordPair[]
                {
                    new("native", "foreign", 1, 3)
                }
            }
        };

        string shouldMatch = 
            $$"""
            [
              {
                "{{nameof(ExportWordCollection.Name)}}": "name",
                "{{nameof(ExportWordCollection.Description)}}": "this is description",
                "{{nameof(ExportWordCollection.LanguageHeaders)}}": "fi-en",
                "{{nameof(ExportWordCollection.WordPairs)}}": [
                  {
                    "{{nameof(ExportWordPair.NativeLanguageWord)}}": "native",
                    "{{nameof(ExportWordPair.ForeignLanguageWord)}}": "foreign",
                    "{{nameof(ExportWordPair.WordLearnStateId)}}": 1,
                    "{{nameof(ExportWordPair.IndexInVocalbulary)}}": 3
                  }
                ]
              }
            ]
            """;

        // Act
        string result = JsonWordCollectionExportService.ConvertDataToJson(collections);

        // Assert
        Assert.Equal(shouldMatch, result);
    }
}
