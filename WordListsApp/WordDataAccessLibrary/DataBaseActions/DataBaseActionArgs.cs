namespace WordDataAccessLibrary.DataBaseActions;
public class DataBaseActionArgs
{
    public DataBaseActionArgs() { }
    public DataBaseActionArgs(string text) 
    {
        Text = text;
    }
    public DataBaseActionArgs(string text, int refId) : this(text)
    {
        RefId = refId;
    }

    public string Text { get; } = string.Empty;
    public int RefId { get; } = -1;
}
