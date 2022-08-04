using System.Xml.Linq;
using WordDataAccessLibrary;
using WordDataAccessLibrary.JsonServices;

namespace WordDataTests.ServiceTests.JsonServices;
public class IJsonWordCollectionTests
{
    readonly IExportWordCollection _jsonCollection = new ExportWordCollection();

    [Theory]
    [InlineData("name", "description", "lang-headers")]
    [InlineData("", "", "")]
    public void FromWordCollection_ShouldUpdate_ObjectInfo(
        string name, 
        string description, 
        string languageHeaders)
    {
        // Arrange
        var jsonCollection = _jsonCollection;
        WordCollection collection = new()
        {
            Owner = new()
            {
                Name = name,
                Description = description,
                LanguageHeaders = languageHeaders
            }
        };

        // Act
        jsonCollection.FromWordCollection(collection);

        // Assert
        Assert.Equal(name, jsonCollection.Name);
        Assert.Equal(description, jsonCollection.Description);
        Assert.Equal(languageHeaders, jsonCollection.LanguageHeaders);
    }

    [Fact]
    public void FromWordCollection_ShouldSet_WordPairs()
    {
        var jsonCollection = _jsonCollection;
        WordCollection collection = new()
        {
            WordPairs = new()
            {
                new(), new(), new()
            }
        };

        // Act
        jsonCollection.FromWordCollection(collection);

        // Assert
        Assert.Equal(3, jsonCollection.WordPairs.Length);
    }
    
    [Fact]
    public void FromWordCollection_ShouldSkip_EmptyWordPairs()
    {
        var jsonCollection = _jsonCollection;
        WordCollection collection = new()
        {
            WordPairs = new(new WordPair[3])
        };

        // Act
        jsonCollection.FromWordCollection(collection);

        // Assert
        Assert.Empty(jsonCollection.WordPairs);
    }
    
    [Fact]
    public void WordPairsFromList_ParamNull_ShouldSet_WordPairs_EmptyArray()
    {
        // Arrange
        var jsonCollection = _jsonCollection;

        // Act
        jsonCollection.FromWordCollection(null);

        // Assert
        Assert.Equal(Array.Empty<ExportWordPair>(), jsonCollection.WordPairs);
    }

    [Fact]
    public void FromWordCollection_ParamNull_ShouldReturnInstantly()
    {
        // Arrange
        var jsonCollection = _jsonCollection;
        jsonCollection.Name = ".";  // Manipulate data to not be default
        int startHash = jsonCollection.GetHashCode();
        
        // Act
        jsonCollection.FromWordCollection(null);

        // Assert
        Assert.Equal(startHash, jsonCollection.GetHashCode());
    }
    
    [Fact]
    public void WordPairsFromList_ParamNull_ShouldReturnInstantly()
    {
        // Arrange
        var jsonCollection = _jsonCollection;
        jsonCollection.Name = ".";  // Manipulate data to not be default
        int startHash = jsonCollection.GetHashCode();

        // Act
        jsonCollection.FromWordCollectionOwner(null);

        // Assert
        Assert.Equal(startHash, jsonCollection.GetHashCode());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void GetAsJson_ShouldThrowException_WhenName_NullOrWhitespace(string name)
    {
        // Arrange
        var jsonCollection = _jsonCollection;

        // Act 
        jsonCollection.Name = name;

        // Assert
        Assert.Throws<InvalidDataException>(() =>
        {
            jsonCollection.GetAsJson();
        });
    }

    [Fact]
    public void GetAsJson_ShouldConvert_ObjectToJson()
    {
        // Arrange
        var jsonCollection = _jsonCollection;
        jsonCollection.Name = nameof(jsonCollection);

        // Act 
        string json = jsonCollection.GetAsJson();

        // Assert
        string matchingString =
            $$"""
            {
              "{{nameof(jsonCollection.Name)}}": "{{nameof(jsonCollection)}}",
              "{{nameof(jsonCollection.Description)}}": "",
              "{{nameof(jsonCollection.LanguageHeaders)}}": "",
              "{{nameof(jsonCollection.WordPairs)}}": []
            }
            """;

        Assert.Equal(matchingString, json);
    }
    
    [Fact]
    public void GetAsJson_ShouldInclude_ExportWordPairs()
    {
        // Arrange
        var jsonCollection = _jsonCollection;
        jsonCollection.Name = nameof(jsonCollection);
        jsonCollection.WordPairsFromList(new()
        {
            new()
            {
                LearnState = WordLearnState.MightKnow,
                IndexInVocalbulary = 7
            }
        });

        // Act 
        string json = jsonCollection.GetAsJson();

        // Assert
        string matchingString =
            $$"""
            {
              "{{nameof(jsonCollection.Name)}}": "{{nameof(jsonCollection)}}",
              "{{nameof(jsonCollection.Description)}}": "",
              "{{nameof(jsonCollection.LanguageHeaders)}}": "",
              "{{nameof(jsonCollection.WordPairs)}}": [
                {
                  "{{nameof(WordPair.NativeLanguageWord)}}": "",
                  "{{nameof(WordPair.ForeignLanguageWord)}}": "",
                  "{{nameof(WordPair.WordLearnStateId)}}": 1,
                  "{{nameof(WordPair.IndexInVocalbulary)}}": 7
                }
              ]
            }
            """;

        Assert.Equal(matchingString, json);
    }

    [Fact]
    public void GetAsJson_ThenParseFromJson_ShouldMatch_StartValue()
    {
        // Arrange
        var jsonCollection = _jsonCollection;
        jsonCollection.Name = nameof(jsonCollection);
        jsonCollection.WordPairsFromList(new()
        {
            new()
            {
                LearnState = WordLearnState.MightKnow,
                IndexInVocalbulary = 7
            }
        });

        // Act 
        string json = jsonCollection.GetAsJson();
        IExportWordCollection resultCollection = ExportWordCollection.ParseFromJson(json);

        // Assert

        Assert.Equal(jsonCollection.Name, resultCollection.Name);
        Assert.Equal(jsonCollection.Description, resultCollection.Description);
        Assert.Equal(jsonCollection.LanguageHeaders, resultCollection.LanguageHeaders);
        Assert.Equal(jsonCollection.WordPairs.First().GetHashCode(), resultCollection.WordPairs.First().GetHashCode());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("this is random string")]
    public void ParseFromJson_WithBadArgument_ShouldReturnNull(string badValue)
    {
        // Arrange & Act 
        IExportWordCollection resultCollection = 
            ExportWordCollection.ParseFromJson(badValue);

        // Assert
        Assert.Null(resultCollection);
    }
}


