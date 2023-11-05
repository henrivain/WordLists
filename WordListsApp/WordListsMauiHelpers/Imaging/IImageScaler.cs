namespace WordListsMauiHelpers.Imaging;
public interface IImageScaler
{
    Task<ImageScaleResult> ScaleDown(string? imagePath, int maxWidth, int maxHeight, string? outputDir = null);
}
