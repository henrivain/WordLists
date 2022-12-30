namespace WordDataAccessLibrary.DataBaseActions;
public class DataBaseActionArgs
{
    public DataBaseActionArgs() { }
    public DataBaseActionArgs(string text) 
    {
        Text = text;
    }
  
    public DataBaseActionArgs(string text, int refId, string collectionName) : this(text)
    {
        RefIds = new[] { refId };
        CollectionNames = new[] { collectionName };
    }

    public DataBaseActionArgs(string text, int[] refIds, string[] collectionNames) : this(text)
    {
        RefIds = refIds;
        CollectionNames = collectionNames;
    }

    public string Text { get; init; } = string.Empty;
    public string[] CollectionNames { get; init; } = Array.Empty<string>();
    public int[] RefIds { get; init; } = Array.Empty<int>();


    public string NameString => string.Join(", ", CollectionNames);
    public string RefIdString => string.Join(", ", RefIds);
}
