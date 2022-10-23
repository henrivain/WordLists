namespace WordDataAccessLibrary.Generators;
public class WordParser
{
    public static List<WordPair> PairWords(string[] words)
    {
        List<WordPair> result = new();
        for (uint i = 0; i + 2 <= words.Length; i += 2)
        {
            result.Add(
                new()
                {
                    ForeignLanguageWord = words[i + 1].Trim(),
                    NativeLanguageWord = words[i].Trim(),
                    IndexInVocalbulary = (int)Math.Round((double)i / 2)
                });
        }
        return result;
    }
}
