// Copyright 2022 Henri Vainio 
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace WordDataAccessLibrary;
public class WordPair
{
    public WordPair() { }

    public WordPair(string nativeLanguageWord, string foreignLanguageWord)
    {
        NativeLanguageWord = nativeLanguageWord;
        ForeignLanguageWord = foreignLanguageWord;
    }

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    string _nativeLanguageWord = string.Empty;
    string _foreignLanguageWord = string.Empty;
    int _indexInVocabulary = -1;

    /// <summary>
    /// Word in language, that user speaks natively
    /// </summary>
    public string NativeLanguageWord
    {
        get => _nativeLanguageWord;
        set => _nativeLanguageWord = value ?? string.Empty;
    }

    /// <summary>
    /// Word in language, that user is trying to learn
    /// </summary>
    public string ForeignLanguageWord
    {
        get => _foreignLanguageWord;
        set => _foreignLanguageWord = value ?? string.Empty;
    }

    public virtual int WordLearnStateId { get; private set; } = (int)WordLearnState.NotSet;

    [EnumDataType(typeof(WordLearnState))]
    public WordLearnState LearnState
    {
        get => (WordLearnState)WordLearnStateId;
        set => WordLearnStateId = (int)value;
    }

    /// <summary>
    /// Index in source vocalbulary, -1 if not defined
    /// </summary>
    public int IndexInVocalbulary 
    {
        get => _indexInVocabulary;
        set
        {
            if (value < -1) 
                throw new ArgumentException($"{IndexInVocalbulary} can't be smaller than -1");
            _indexInVocabulary = value;
        } 
    }

    /// <summary>
    /// Id of WordCollectionInfo owning this word pait
    /// </summary>
    public int OwnerId { get; set; } = -1;
}
