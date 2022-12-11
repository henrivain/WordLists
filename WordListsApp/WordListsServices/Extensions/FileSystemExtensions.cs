using WordListsServices.FileSystemServices.ActionResults;

namespace WordListsServices.Extensions;
internal static class FileSystemExtensions
{
    internal static bool NotSuccess(this ActionResult result) => result.Success is false;
}
