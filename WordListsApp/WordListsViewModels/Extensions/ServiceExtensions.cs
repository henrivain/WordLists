using WordListsServices;

namespace WordListsViewModels.Extensions;
internal static class ServiceExtensions
{
    internal static bool NotSuccess(this IActionResult result)
    {
        return result.Success is false;
    }
}
