using SQLite;

namespace WordDataAccessLibrary;
public class WordCollection
{
    public WordCollection() { }

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = "My word collection";

    public string Description { get; set; } = string.Empty;

    public WordPair[] WordPairs { get; set; } = Array.Empty<WordPair>();
}
