namespace WordDataAccessLibrary.CollectionBackupServices;

public interface IExportWordCollection
{
    string Description { get; set; }
    string LanguageHeaders { get; set; }
    string Name { get; set; }
    ExportWordPair[] WordPairs { get; set; }
    IExportWordCollection FromWordCollection(WordCollection collection);
    IExportWordCollection FromWordCollectionOwner(WordCollectionOwner owner);
    IExportWordCollection WordPairsFromList(List<WordPair> pairs);
    WordCollection GetAsWordCollection();
}