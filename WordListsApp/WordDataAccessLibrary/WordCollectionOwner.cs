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

    /// <summary>
    /// Short identifyer for what languages are used in collection
    /// <para/>Example fi-en => native: finnish, foreign: english
    /// <para/>Example sw-fi => native: swedish, foreign: finnish
    /// <para/>Default empty string 
    /// </summary>
    public string LanguageHeaders { get; set; } = string.Empty;
}
