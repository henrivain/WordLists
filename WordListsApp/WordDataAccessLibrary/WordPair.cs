// Copyright 2021 Henri Vainio 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace WordDataAccessLibrary;
public class WordPair
{
    public string NativeLanguageWord { get; set; } = "native word";
    
    public string ForeignLanguageWord { get; set; } = "foreign word";
    
    public WordLearnState WordLearnState { get; set; } = WordLearnState.NeverHeard;

    public uint IndexInVocalbulary { get; set; } = 0;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

}
