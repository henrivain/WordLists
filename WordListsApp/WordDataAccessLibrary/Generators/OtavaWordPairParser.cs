// Copyright 2022 Henri Vainio 

namespace WordDataAccessLibrary.Generators;

public class OtavaWordPairParser : WordParser, IWordPairParser
{
    public virtual List<string> ToStringList(string vocabulary)
    {
        if (string.IsNullOrWhiteSpace(vocabulary))
        {
            return new();
        }

        string[] lines = vocabulary
            .Replace('\r', '\n')
            .Split('\n')
            .ToArray();

        return CleanLines(lines).ToList();
    }
    public virtual List<WordPair> GetList(string vocabulary)
    {
        return PairWords(ToStringList(vocabulary).ToArray());
    }

    private protected static string[] CleanLines(string[] lines)
    {
        return lines.Select(RemovePronunciation)
                    .Where(x => string.IsNullOrWhiteSpace(x) is false)
                    .Select(x => x.Trim())
                    .ToArray();
    }
    private protected static string RemovePronunciation(string line)
    {
        while (true)
        {
            int start = line.IndexOf('[');
            if (start < 0) return line;

            int end = line.IndexOf("]", start);
            if (end < 0) return line;
            
            line = $"{line[..start]}{line[(end + 1)..]}".Trim();
        }
    }
}
