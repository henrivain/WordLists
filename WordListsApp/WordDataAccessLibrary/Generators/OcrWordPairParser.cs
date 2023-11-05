namespace WordDataAccessLibrary.Generators;
public class OcrWordPairParser : WordParser, IOcrWordPairParser
{
    public List<WordPair> GetList(string vocabulary)
    {
        ToLineHalves(vocabulary, out var firstHalf, out var secondHalf);
        int maxIndex = Math.Min(firstHalf.Length, secondHalf.Length);
        
        List<WordPair> result = new();
        for (int i = 0; i < maxIndex; i++)
        {
            result.Add(new()
            {
                NativeLanguageWord = firstHalf[i],
                ForeignLanguageWord = secondHalf[i],
                IndexInVocalbulary = i
            });
        }
        return result;
    }
    public List<string> ToStringList(string vocabulary)
    {
        ToLineHalves(vocabulary, out var firstHalf, out var secondHalf);
        int maxIndex = Math.Min(firstHalf.Length, secondHalf.Length);

        List<string> result = new();
        for (int i = 0; i < maxIndex; i++)
        {
            result.Add(firstHalf[i]);
            result.Add(secondHalf[i]);
        }
        return result;
    }

    private static void ToLineHalves(string vocabulary, out string[] firstHalf, out string[] secondHalf)
    {
        var lines = vocabulary
                    .Replace('\r', '\n')
                    .Replace("\n\n", "\n")
                    .Split('\n')
                    .Select(RemovePronunciation)
                    .CleanLines()
                    .ToArray();

        int middleIndex = (lines.Length / 2);

        firstHalf = lines[..middleIndex].ToArray();
        secondHalf = lines[middleIndex..].ToArray();
    }


    private protected static new string RemovePronunciation(string line)
    {
        int start = 0;
        while (true)
        {
            start = line.IndexOfAny(new char[] { '[', '(', '{' }, start);
            if (start < 0) return line;

            int end = line.IndexOfAny(new char[] { ']', ')', '}' }, start);
            if (end < 0) return line;

            line = $"{line[..start]}{line[(end + 1)..]}";
        }
    }
}
