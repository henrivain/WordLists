// Copyright 2022 Henri Vainio 

namespace WordDataAccessLibrary.Generators;

public class OtavaWordPairParser : WordParser, IWordPairParser
{
    public OtavaWordPairParser(string vocabularyList)
    {
        VocabularyList = vocabularyList;
    }
    private string VocabularyList { get; set; }
    public List<string> ToStringList()
    {
        var pairs = GetList();
        List<string> result = Enumerable.Empty<string>().ToList();

        foreach(var pair in pairs)
        {
            result.Add(pair.NativeLanguageWord);
            result.Add(pair.ForeignLanguageWord);
        }
        return result;
    }
    public List<WordPair> GetList()
    {
        if (string.IsNullOrWhiteSpace(VocabularyList)) return new();

        string[] lines = VocabularyList.Replace('\r', '\n').Split('\n');
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
