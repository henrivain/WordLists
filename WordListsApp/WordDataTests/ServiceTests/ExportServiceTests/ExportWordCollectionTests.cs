using WordDataAccessLibrary;
using WordDataAccessLibrary.CollectionBackupServices;

namespace WordDataTests.ServiceTests.ExportServiceTests;
public class ExportWordCollectionTests
{
    readonly IExportWordCollection _exportWordCollection = new ExportWordCollection();

    [Theory]
    [InlineData("name", "description", "lang-headers")]
    [InlineData("", "", "")]
    public void FromWordCollection_ShouldUpdate_ObjectInfo(
        string name,
        string description,
        string languageHeaders)
    {
        // Arrange
        var jsonCollection = _exportWordCollection;
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
        var jsonCollection = _exportWordCollection;
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
        var jsonCollection = _exportWordCollection;
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
        var jsonCollection = _exportWordCollection;

        // Act
        jsonCollection.FromWordCollection(null);

        // Assert
        Assert.Equal(Array.Empty<ExportWordPair>(), jsonCollection.WordPairs);
    }

    [Fact]
    public void FromWordCollection_ParamNull_ShouldReturnInstantly()
    {
        // Arrange
        var jsonCollection = _exportWordCollection;
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
        var jsonCollection = _exportWordCollection;
        jsonCollection.Name = ".";  // Manipulate data to not be default
        int startHash = jsonCollection.GetHashCode();

        // Act
        jsonCollection.FromWordCollectionOwner(null);

        // Assert
        Assert.Equal(startHash, jsonCollection.GetHashCode());
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


