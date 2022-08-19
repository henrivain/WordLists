using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WordValidationLibraryTests")]

namespace WordValidationLibrary;

public class UserInputWordValidator : IUserInputWordValidator
{
	public WordMatchResult CompareWords(string userInput, string correct)
    {
        if (correct is null) throw new ArgumentNullException(nameof(correct));
        if (userInput is null) throw new ArgumentNullException(nameof(userInput));
        if (userInput == correct) return new(true)
        {
            CharMatchPercentage = 100,
            ValidatedStringSpans = new() { new(userInput, true) }
        };

        string input;
        bool[] validatedChars;

        (input, validatedChars) = GetCorrectCharsInInput(userInput, correct);

        List<MatchingString> matchingStrings = CreateFromBoolArray(input, validatedChars);



        return new(false)
        {
            ValidatedStringSpans = matchingStrings,
            HasMatch = matchingStrings.Any(x => x.IsMatch is true),
            CharMatchPercentage = GetMatchPercentage(matchingStrings, correct)
        };
    }

    private static ushort GetMatchPercentage(List<MatchingString> matchingStrings, string correct)
    {
        int correctChars = 0;
        foreach (var matchingString in matchingStrings)
        {
            if (matchingString.IsMatch)
            {
                correctChars += matchingString.String.Length;
            }
        }
        if (correct.Length <= 0) return 100;
        return (ushort)((double)correctChars / correct.Length * 100);
    }



    internal static (string Input, bool[] ValidChars) GetCorrectCharsInInput(string input, string correct)
    {
        int maxCorrectIndex = Math.Min(input.Length, correct.Length);

        bool[] validChars = new bool[input.Length];
        Array.Fill(validChars, false);

        int i = 0;
        int g = i;
        while (i < maxCorrectIndex && g < input.Length)
        {
            if (input[g] == correct[i])
            {
                validChars[g] = true;
            }
            else if (char.ToLower(input[g]) == char.ToLower(correct[i]))
            {
                // This char is false, because case does not match,
                // but algorithm does not try to find new char with same case
            }
            else
            {
                int index = input[g..].IndexOf(correct[i]);
                if (index is not -1)
                {
                    index += g;
                    validChars[index] = true;
                    g = index;
                }
            }
            g++;
            i++;
        }
        return (input, validChars);
    }

    internal static List<MatchingString> CreateFromBoolArray(string validatedString, bool[] matches)
    {
        if (validatedString.Length != matches.Length)
        {
            throw new InvalidDataException(
                $"{nameof(validatedString)}'s and {nameof(validatedString)}'s " +
                $"lengths do not match. " +
                $"\n\t{nameof(validatedString)}: {validatedString.Length}, " +
                $"\n\t{nameof(matches)}: {matches.Length}");
        }
        if (matches.Length <= 0)
        {
            return new();
        }

        List<MatchingString> result = new();

        int startIndex = 0;

        bool IsPreviousCharMatch = matches[0];
        int i;
        for (i = 0; i < matches.Length; i++)
        {
            if (IsPreviousCharMatch == matches[i]) continue;

            result.Add(new(validatedString[startIndex..i], IsPreviousCharMatch));
            IsPreviousCharMatch = matches[i];
            startIndex = i;
        }
        bool lastNotAdded = i > 1 && matches[i - 1] != matches[i - 2];
        bool lastSpanNotAdded = startIndex < i - 1;

        if (lastSpanNotAdded || lastNotAdded)
        {
            result.Add(new(validatedString[startIndex..], IsPreviousCharMatch));
        }
        return result;
    }
}
