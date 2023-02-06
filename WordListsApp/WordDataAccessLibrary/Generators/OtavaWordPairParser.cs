// Copyright 2022 Henri Vainio 

namespace WordDataAccessLibrary.Generators;

public class OtavaWordPairParser : WordParser, IWordPairParser
{
    public List<string> ToStringList(string vocabulary)
    {
        var pairs = GetList(vocabulary);
        List<string> result = new();

        foreach(var pair in pairs)
        {
            result.Add(pair.NativeLanguageWord);
            result.Add(pair.ForeignLanguageWord);
        }
        return result;
    }
    public List<WordPair> GetList(string vocabulary)
    {
        if (string.IsNullOrWhiteSpace(vocabulary))
        {
            return new();
        }
        string[] lines = vocabulary.Replace('\r', '\n').Split('\n');
        lines = CleanLines(lines);
        return PairWords(lines);
    }

    private static string[] CleanLines(string[] lines)
    {
        return lines.Select(RemovePronunciation)
                    .Where(x => string.IsNullOrWhiteSpace(x) is false)
                    .Select(x => x.Trim())
                    .ToArray();
    }
    private static string RemovePronunciation(string line)
    {
        int start = line.IndexOf('[');
        if (start < 1) return line;

        int end = line.IndexOf("]");
        if (end < 1) return line;

        return string.Concat(line[..start], line[(end + 1)..]).Trim();
    }
}
