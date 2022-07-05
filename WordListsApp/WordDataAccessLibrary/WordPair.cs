// Copyright 2022 Henri Vainio 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace WordDataAccessLibrary;
public class WordPair
{
    public WordPair() { }

    public WordPair(string nativeLanguageWord, string foreignLanguageWord)
    {
        NativeLanguageWord = nativeLanguageWord;
        ForeignLanguageWord = foreignLanguageWord;
    }

    public string NativeLanguageWord { get; set; } = string.Empty;
    
    public string ForeignLanguageWord { get; set; } = string.Empty;

    public WordLearnState WordLearnState { get; set; } = WordLearnState.NeverHeard;

    public uint IndexInVocalbulary { get; set; } = 0;
}
