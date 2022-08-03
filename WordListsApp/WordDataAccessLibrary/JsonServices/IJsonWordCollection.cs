namespace WordDataAccessLibrary.JsonServices;

public interface IJsonWordCollection
{
    string Description { get; set; }
    string LanguageHeaders { get; set; }
    string Name { get; set; }
    ExportWordPair[] WordPairs { get; set; }

    IJsonWordCollection FromWordCollection(WordCollection collection);
    IJsonWordCollection FromWordCollectionOwner(WordCollectionOwner owner);
    IJsonWordCollection WordPairsFromList(List<WordPair> pairs);
    string GetAsJson();
}