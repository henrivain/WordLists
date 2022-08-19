namespace WordValidationLibrary;

public interface IUserInputWordValidator
{
    WordMatchResult CompareWords(string userInput, string correct);
}