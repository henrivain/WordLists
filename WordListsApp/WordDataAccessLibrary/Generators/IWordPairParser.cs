// Copyright 2022 Henri Vainio 

namespace WordDataAccessLibrary.Generators;
public interface IWordPairParser
{
    List<WordPair> GetList(string vocabulary);
    List<string> ToStringList(string vocabulary);
}
