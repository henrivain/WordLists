namespace WordDataAccessLibrary.CollectionBackupServices;
public class ExportWordPair
{
    public ExportWordPair(string nativeLanguageWord, string foreignLanguageWord, int wordLearnStateId, int indexInVocalbulary = -1)
    {
        NativeLanguageWord = nativeLanguageWord ?? string.Empty;
        ForeignLanguageWord = foreignLanguageWord ?? string.Empty;
        WordLearnStateId = wordLearnStateId;
        IndexInVocalbulary = indexInVocalbulary < -1 ? -1 : indexInVocalbulary;
    }

    public string NativeLanguageWord { get; }
    public string ForeignLanguageWord { get; }
    public int WordLearnStateId { get; }
    public int IndexInVocalbulary { get; }

    public override bool Equals(object obj)
    {
        return obj is ExportWordPair pair &&
               NativeLanguageWord == pair.NativeLanguageWord &&
               ForeignLanguageWord == pair.ForeignLanguageWord &&
               WordLearnStateId == pair.WordLearnStateId &&
               IndexInVocalbulary == pair.IndexInVocalbulary;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(NativeLanguageWord, ForeignLanguageWord, WordLearnStateId, IndexInVocalbulary);
    }
    public static ExportWordPair FromWordPair(WordPair pair)
    {
        return new(pair.NativeLanguageWord,
            pair.ForeignLanguageWord,
            pair.WordLearnStateId,
            pair.IndexInVocalbulary);
    }
}
