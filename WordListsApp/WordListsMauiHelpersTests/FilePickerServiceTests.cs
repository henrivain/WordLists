﻿
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
        List<string> result = FilePickerService.GetValidFileExtensions(extensions);

        // Assert
        Assert.Equal(shouldMatch, result);
    }

    [Fact]
    public void ValidateFileExtensions_WithEmptyList_ShouldReturn_EmptyList()
    {
        // Arrange
        List<string> extensions = new();

        // Act
        List<string> result = FilePickerService.GetValidFileExtensions(extensions);

        // Assert
        Assert.Equal(new List<string>(), result);
    }
    
    [Fact]
    public void ValidateFileExtensions_WithNull_ShouldReturn_EmptyList()
    {
        // Arrange & Act
        List<string> result = FilePickerService.GetValidFileExtensions(null);

        // Assert
        Assert.Equal(new List<string>(), result);
    }


}