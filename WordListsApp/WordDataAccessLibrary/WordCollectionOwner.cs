using SQLite;

namespace WordDataAccessLibrary;
public class WordCollectionOwner
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Short name, for example "English Chapter 2"
    /// </summary>
    public string Name { get; set; } = "My word collection";

    /// <summary>
    /// Topic of these words
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
