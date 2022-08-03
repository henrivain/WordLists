namespace WordDataAccessLibrary.JsonServices;

public class JsonActionResult
{
    public JsonActionResult(JsonAction actionType)
    {
        ActionType = actionType;
    }

    public JsonAction ActionType { get; }

    public string UsedPath { get; set; } = string.Empty;

    public bool Success { get; set; } = false;

    public string ModeInfo { get; set; } = string.Empty;
}