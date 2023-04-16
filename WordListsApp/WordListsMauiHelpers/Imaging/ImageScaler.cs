using Microsoft.Extensions.Logging;
using SkiaSharp;
using static WordListsMauiHelpers.Imaging.ImageStatus;

namespace WordListsMauiHelpers.Imaging;
public class ImageScaler : IImageScaler
{
    public ImageScaler(ILogger<IImageScaler> logger)
    {
        Logger = logger;
    }

    ILogger<IImageScaler> Logger { get; }

    public async Task<ImageScaleResult> ScaleDown(string? inputImagePath, int maxWidth, int maxHeight)
    {
        Logger.LogInformation("Scale down image from '{path}'.", inputImagePath);

        if (string.IsNullOrWhiteSpace(inputImagePath))
        {
            Logger.LogWarning("Image path is null or empty, cannot scale.");
            return new ImageScaleResult
            {
                Success = InvalidFile,
                ImagePath = null,
                Message = "Null or empty path."
            };
        }
        if (File.Exists(inputImagePath) is false)
        {
            Logger.LogWarning("Image file does not exist, cannot scale.");
            return new ImageScaleResult
            {
                Success = InvalidFile,
                ImagePath = inputImagePath,
                Message = "File does not exist."
            };
        }


        byte[] bytes;
        try
        {
            bytes = File.ReadAllBytes(inputImagePath);
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Cannot read image bytes from, '{ex}': '{msg}'.",
                ex.GetType().Name, ex.Message);
            return new ImageScaleResult
            {
                Success = LoadFailed,
                ImagePath = inputImagePath,
                Message = $"Cannot image file '{ex.GetType()}': '{ex.Message}'",
            };
        }
        if (bytes is null)
        {
            Logger.LogWarning("Image had no bytes, Cannot scale empty image.");
            return new ImageScaleResult
            {
                Success = LoadFailed,
                ImagePath = inputImagePath,
                Message = "Image was empty, no bytes."
            };
        }


        using Stream input = new MemoryStream(bytes);
        if (input?.Length <= 0)
        {
            Logger.LogWarning("Could not read image bytes as memory stream, stream empty.");
            return new ImageScaleResult
            {
                Success = LoadFailed,
                ImagePath = inputImagePath,
                Message = "Image as memory stream was empty."
            };
        }


        using var image = SKImage.FromEncodedData(input);
        if (image is null)
        {
            Logger.LogWarning("Error whilst reading encoded image data.");
            return new ImageScaleResult
            {
                Success = LoadFailed,
                ImagePath = inputImagePath,
                Message = "Could not read image data, image might be corrupt."
            };
        }

        using SKBitmap bitmap = SKBitmap.FromImage(image);
        if (bitmap is null)
        {
            Logger.LogWarning("Could not create bitmap from image.");
            return new ImageScaleResult
            {
                Success = LoadFailed,
                ImagePath = inputImagePath,
                Message = "Could not create bitmap from image."
            };
        }
        if (bitmap.Width < maxWidth && bitmap.Height < maxHeight)
        {
            Logger.LogInformation("Image is already small enough width dimensions '{h}x{w}'.",
                bitmap.Height, bitmap.Width);
            return new ImageScaleResult
            {
                Success = Success,
                ImagePath = inputImagePath,
                Height = bitmap.Height,
                Width = bitmap.Width,
                Message = "Input was small enough."
            };
        }

        float heightMinRatio = bitmap.Height / (float)maxHeight;
        float widthMinRatio = bitmap.Width / (float)maxWidth;
        float ratio = Math.Min(heightMinRatio, widthMinRatio);

        int targetHeight = bitmap.Height;
        int targetWidth = bitmap.Height;

        // Calculate final height and width

        var outputInfo = new SKImageInfo()
        {
            Height = targetHeight,
            Width = targetWidth
        };

        using SKBitmap outputBitmap = bitmap.Resize(outputInfo, SKFilterQuality.Medium);
        if (outputBitmap is null)
        {
            Logger.LogWarning("Failed to resize image.");
            return new ImageScaleResult
            {
                Success = InvalidProcess,
                ImagePath = inputImagePath,
                Message = "Failed to resize image.",
                Height = targetHeight,
                Width = targetWidth
            };
        }

        using SKData outputData = outputBitmap.Encode(SKEncodedImageFormat.Png, 70);
        if (outputData is null)
        {
            Logger.LogWarning("Failed to encode scaled image to png.");
            return new ImageScaleResult
            {
                Success = InvalidProcess,
                Message = "Failed encode image to png.",
                ImagePath = inputImagePath,
                Height = targetHeight,
                Width = targetWidth
            };
        }

        string fileName = Path.GetFileNameWithoutExtension(inputImagePath);
        string outputPath = Path.Combine(FileSystem.Current.CacheDirectory, "scaling", $"{fileName ?? "scaled"}.png");

        try
        {
            using Stream outputStream = File.OpenWrite(outputPath);
            await outputData.AsStream().CopyToAsync(outputStream);
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Could not save scaled image, '{ex}': '{msg}'.", ex.GetType().Name, ex.Message);
            return new ImageScaleResult
            {
                Success = SaveFailed,
                Message = $"Could not save scaled image, '{ex.GetType()}': '{ex.Message}'.",
                ImagePath = inputImagePath,
                Height = targetHeight,
                Width = targetWidth
            };
        }
        Logger.LogInformation("Scaled image successfully, output at '{path}'.", outputPath);
        return new ImageScaleResult
        {
            Success = NotImplemented,
            ImagePath = outputPath,
            Message = $"Successfully scaled to '{targetHeight}x{targetWidth}'",
            Height = targetHeight,
            Width = targetWidth
        };
    }
}
