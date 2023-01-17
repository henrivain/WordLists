using WordDataAccessLibrary.CollectionBackupServices;

namespace WordDataAccessLibrary.Extensions;
public static class ActionResultExtensions
{
    public static bool NotSuccess(this ImportActionResult result)
    {
        return result.Success is false;
    }
}
