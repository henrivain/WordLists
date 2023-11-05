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

        return vocabulary
            .Replace('\r', '\n')
            .Split('\n')
            .CleanLines()
            .ToList();
    }
    public virtual List<WordPair> GetList(string vocabulary)
    {
        return PairWords(ToStringList(vocabulary).ToArray());
    }
}
