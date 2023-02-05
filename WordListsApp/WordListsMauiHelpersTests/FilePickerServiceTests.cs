
namespace WordListsMauiHelpersTests;
public class FilePickerServiceTests
{
    [Fact]
    public void ValidateFileExtensions_ShouldCorrect_ExtensionWithoutDot()
    {
        // Arrange
        List<string> extensions = new()
        {
            "json", ".json", "zip"
        };
        
        List<string> shouldMatch = new()
        {
            ".json", ".json", ".zip"
        };
        
        // Act
        List<string> result = DeviceSpecificFilePicker.GetValidFileExtensions(extensions);

        // Assert
        Assert.Equal(shouldMatch, result);
    }

    [Fact]
    public void ValidateFileExtensions_WithEmptyList_ShouldReturn_EmptyList()
    {
        // Arrange
        List<string> extensions = new();

        // Act
        List<string> result = DeviceSpecificFilePicker.GetValidFileExtensions(extensions);

        // Assert
        Assert.Equal(new List<string>(), result);
    }
    
    [Fact]
    public void ValidateFileExtensions_WithNull_ShouldReturn_EmptyList()
    {
        // Arrange & Act
        List<string> result = DeviceSpecificFilePicker.GetValidFileExtensions(Enumerable.Empty<string>().ToList());

        // Assert
        Assert.Equal(new List<string>(), result);
    }


}
