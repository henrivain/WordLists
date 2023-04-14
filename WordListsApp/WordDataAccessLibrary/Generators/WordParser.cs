namespace WordDataAccessLibrary.Generators;
public class WordParser
{
    /// <summary>
    /// Pairs string with next string to word pair, also sets pair index.
    /// </summary>
    /// <param name="words"></param>
    /// <returns>List of word pairs.</returns>
    public static List<WordPair> PairWords(string[] words)
    {
        List<WordPair> result = new();

        for (uint i = 0; i + 2 <= words.Length; i += 2)
        {
            result.Add(
                new()
                {
                    NativeLanguageWord = words[i].Trim(),
                    ForeignLanguageWord = words[i + 1].Trim(),
                    IndexInVocalbulary = (int)Math.Round((double)i / 2)
                });
        }
        return result;
    }

   

    private protected static string RemovePronunciation(string line)
    {
        while (true)
        {
            int start = line.IndexOf('[');
            if (start < 0) return line;

            int end = line.IndexOf("]", start);
            if (end < 0) return line;

            line = $"{line[..start]}{line[(end + 1)..]}";
        }
    }
}
