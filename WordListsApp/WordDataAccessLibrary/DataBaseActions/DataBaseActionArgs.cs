namespace WordDataAccessLibrary.DataBaseActions;
public class DataBaseActionArgs
{
    public DataBaseActionArgs() { }
    public DataBaseActionArgs(string text) 
    {
        Text = text;
    }
    public DataBaseActionArgs(int refId) 
    {
        RefId = refId;
    }
    public DataBaseActionArgs(string text, int refId) : this(text)
    {
        RefId = refId;
    }
    public DataBaseActionArgs(string text, int refId, string collectionName) : this(text, refId)
    {
        CollectionName = collectionName;
    }
    public DataBaseActionArgs(int refId, string text = "", string collectionName = "")
    {
        RefId = refId;
        Text = text;
        CollectionName = collectionName;
    }

    public string Text { get; } = string.Empty;
    public string CollectionName { get; } = string.Empty;
    public int RefId { get; } = -1;
}
