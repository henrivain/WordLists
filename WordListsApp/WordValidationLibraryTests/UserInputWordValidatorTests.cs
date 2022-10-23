using WordValidationLibrary;

namespace WordValidationLibraryTests;

public class UserInputWordValidatorTests
{
    readonly UserInputWordValidator _validator = new();

    [Theory]
    [InlineData(" ", " ")]
    [InlineData("", "")]
    [InlineData("Input", "Input")]
    [InlineData("G", "G")]
    [InlineData("g", "g")]
    [InlineData("with space", "with space")]
    public void CompareWords_ShouldReturnTrue_IfWordsMatch(string input, string correct)
    {
        // Arrange & Act
        WordMatchResult result = _validator.CompareWords(input, correct);

        // Assert
        Assert.True(result.IsFullMatch);
    }

    [Theory]
    [InlineData("", "text")]
    [InlineData("", " ")]
    [InlineData("nice words", "just word")]
    public void CompareWords_ShouldReturnFalse_IfWordsNotMatch(string input, string correct)
    {
        // Arrange & Act
        WordMatchResult result = _validator.CompareWords(input, correct);

        // Assert
        Assert.False(result.IsFullMatch);
    }

    [Theory]
    [InlineData(null, "string")]
    [InlineData("string", null)]
    [InlineData(null, null)]
    public void CompareWords_WithNull_ShouldThrowArgumentNullException(string input, string correct)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _validator.CompareWords(input, correct);
        });
    }

    [Theory]
    [InlineData("text", "Text")]
    [InlineData("Das auto", "das Auto")]
    [InlineData("Fun", "fun")]
    public void CompareWord_ShouldReturn_HasMatches_IfCasingWrong(string input, string correct)
    {
        // Arrange & Act
        WordMatchResult result = _validator.CompareWords(input, correct);

        // Assert
        Assert.False(result.IsFullMatch);
        Assert.True(result.HasMatch);
    }

    [Theory]
    [InlineData("aag", "aga", "[1,0,1]")]
    [InlineData("aga", "aGa", "[1,0,1]")]
    [InlineData("Das auto", "das Auto", "[0,1,1,1,0,1,1,1]")]
    [InlineData("der Auto", "das Auto", "[1,0,0,1,1,1,1,1]")]
    [InlineData("gurguma", "gguma", "[1,0,0,1,1,1,1]")]
    [InlineData("ffpp", "gppp", "[0,0,1,1]")]
    [InlineData("G", "g", "[0]")]
    public void GetCorrectCharsInInput_ShouldReturn_FirstAppearenceIndex_FromInput(
        string input, 
        string correct, 
        string resultArray)
    {
        // Arrange
        string returnedInput;
        bool[] validChars;

        // Act
        (returnedInput, validChars) = UserInputWordValidator.GetCorrectCharsInInput(input, correct);

        string parsedArray = string.Join(",", Array.ConvertAll(validChars, x => x).Select(x => x ? 1 : 0));
        parsedArray = $"[{parsedArray}]";

        // Assert
        Assert.Equal(input, returnedInput);
        Assert.Equal(resultArray, parsedArray);
    }



    [Fact]
    public void CreateFromBoolArray_ShouldReturn_ListOfMatchingString_WithRightState()
    {
        // Arrange
        string input = "aabbccdd";
        string correct = "bbcd";
        string returnedInput;
        bool[] validChars;
        List<MatchingString> expectedResult = new()
        {
            new("aa", false),
            new("bbc", true),
            new("c", false),
            new("d", true),
            new("d", false),
        };

        // Act


        (returnedInput, validChars) = UserInputWordValidator.GetCorrectCharsInInput(input, correct);

        List<MatchingString> strings = UserInputWordValidator.CreateFromBoolArray(returnedInput, validChars);


        // Assert
        Assert.Equal(input, returnedInput);
        Assert.Equal(expectedResult.ToJsonString(), strings.ToJsonString());
    }

    [Theory]
    [InlineData("input", "input", 100)]
    [InlineData("inpu", "input", 80)]
    [InlineData("", "input", 0)]
    public void CompareWords_ShouldSet_CharMatchPercentage(
        string input,
        string correct,
        ushort percentage)
    {
        // Arrange & Act
        ushort resultPercentage = _validator.CompareWords(input, correct).CharMatchPercentage;

        // Assert
        Assert.Equal(percentage, resultPercentage);
    }
    
}