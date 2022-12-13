namespace WordListsServices;
public class ActionResult : IActionResult
{
    public ActionResult(bool success)
    {
        Success = success;
    }
    public ActionResult(bool success, string message) : this(success)
    {
        Message = message;
    }

    string _message = string.Empty;

    public bool Success { get; }
    public virtual string Message
    {
        get => _message;
        init
        {
            if (value is null)
            {
                _message = string.Empty;
                return;
            }
            _message = value;
        }
    }
}
