namespace WordDataAccessLibrary.JsonServices;

public interface IJsonWordCollection
{
    string Description { get; set; }
    string LanguageHeaders { get; set; }
    string Name { get; set; }
    ExportWordPair[] WordPairs { get; set; }

    void FromWordCollection(WordCollection collection);
    void FromWordCollectionOwner(WordCollectionOwner owner);
    string GetAsJson();
    void WordPairsFromList(List<WordPair> pairs);
}