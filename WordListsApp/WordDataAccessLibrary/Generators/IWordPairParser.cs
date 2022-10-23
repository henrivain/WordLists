// Copyright 2022 Henri Vainio 

namespace WordDataAccessLibrary.Generators;
public interface IWordPairParser
{
    List<WordPair> GetList();

    List<string> ToStringList();
}
