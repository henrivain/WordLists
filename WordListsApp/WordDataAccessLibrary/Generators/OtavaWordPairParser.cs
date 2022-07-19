// Copyright 2022 Henri Vainio 

using System.Runtime.CompilerServices;

namespace WordDataAccessLibrary.Generators;

public class OtavaWordPairParser : IWordPairParser
{
    public OtavaWordPairParser(string vocabularyList)
    {
        VocabularyList = vocabularyList;
    }
    private string VocabularyList { get; set; }
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
    private static List<WordPair> PairWords(string[] lines)
    {
        List<WordPair> result = new();
        for (uint i = 0; i + 2 <= lines.Length; i += 2)
        {
            result.Add(
                new()
                {
                    // in otava books foreign language is first and native second
                    ForeignLanguageWord = lines[i + 1].Trim(),
                    NativeLanguageWord = lines[i].Trim(),
                    IndexInVocalbulary = (int)Math.Round((double)i / 2)
                });
        }
        return result;
    }
}
