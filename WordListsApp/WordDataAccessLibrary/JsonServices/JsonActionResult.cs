namespace WordDataAccessLibrary.JsonServices;

public class JsonActionArgs
{
    public JsonActionArgs(JsonAction actionType)
    {
        ActionType = actionType;
    }

    public JsonAction ActionType { get; }

    public string UsedPath { get; set; } = string.Empty;

    public bool Success { get; set; } = false;

    public string MoreInfo { get; set; } = string.Empty;
}