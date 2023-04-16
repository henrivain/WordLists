using WordListsMauiHelpers.Imaging;

namespace WordListsMauiHelpers.Extensions;
public static class ResultExtensions
{
    public static bool Success(this ImageScaleResult result) => result.Success is ImageStatus.Success;
    public static bool NotSuccess(this ImageScaleResult result) => result.Success() is false;

}
