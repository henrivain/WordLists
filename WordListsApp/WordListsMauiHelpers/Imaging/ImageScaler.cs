using Microsoft.Extensions.Logging;
using SkiaSharp;
using WordListsMauiHelpers.Extensions;
using static WordListsMauiHelpers.Imaging.ImageStatus;

namespace WordListsMauiHelpers.Imaging;
public class ImageScaler : IImageScaler
{
    public ImageScaler(ILogger<IImageScaler> logger)
    {
        Logger = logger;
    }

    public static string DefaultOutputDir { get; } = Path.Combine(FileSystem.Current.CacheDirectory, "scaling");

    ILogger<IImageScaler> Logger { get; }

    public async Task<ImageScaleResult> ScaleDown(string? inputImagePath, int maxWidth, int maxHeight, string? outputDir = null)
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
            Logger.LogException("Cannot read image bytes from image", ex);

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

        // Calculate the new width and height
        float heightMultiplyer = GetScaleMultiplyer(bitmap.Height, maxHeight);
        float widthMultiplyer = GetScaleMultiplyer(bitmap.Width, maxWidth);

        float multiplyer = Math.Min(heightMultiplyer, widthMultiplyer);

        int targetHeight = (int)(bitmap.Height * (double)multiplyer);
        int targetWidth = (int)(bitmap.Width * (double)multiplyer);

        Logger.LogInformation("Scaling image from '{h}x{w}' to '{th}x{tw}'.",
            bitmap.Height, bitmap.Width, targetHeight, targetWidth);

        var outputInfo = new SKImageInfo(targetWidth, targetHeight);


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
        
        string outputFile = GetOutputFilePath(inputImagePath, outputDir, targetHeight, targetWidth);

        using Stream? outputStream = CreateOrOverwrite(outputFile);
        if (outputStream is null)
        {
            Logger.LogWarning("Could not create output file or delete old entry.");
            return new ImageScaleResult
            {
                Success = SaveFailed,
                ImagePath = inputImagePath,
                Message = "Could not create output file or delete old entry."
            };
        }
        try
        {
            await outputData.AsStream().CopyToAsync(outputStream);
        }
        catch (Exception ex)
        {
            Logger.LogException("Could not save scaled image", ex);
            return new ImageScaleResult
            {
                Success = SaveFailed,
                Message = $"Could not save scaled image, '{ex.GetType()}': '{ex.Message}'.",
                ImagePath = inputImagePath,
                Height = targetHeight,
                Width = targetWidth
            };
        }
        Logger.LogInformation("Scaled image successfully, output at '{path}'.", outputFile);
        return new ImageScaleResult
        {
            Success = Success,
            ImagePath = outputFile,
            Message = $"Successfully scaled to '{targetHeight}x{targetWidth}'",
            Height = targetHeight,
            Width = targetWidth
        };
    }

    private string GetOutputFilePath(string inputPath, string? outputDir, int targetHeight, int targetWidth)
    {
        outputDir = GetValidOutputDirectory(outputDir);

        string fileName = Path.GetFileNameWithoutExtension(inputPath);
        string outputFile = Path.Combine(outputDir, $"{fileName ?? "image"}_scaled_{targetHeight}x{targetWidth}.png");
        return outputFile;
    }

    private string GetValidOutputDirectory(string? outputDir)
    {
        if (string.IsNullOrWhiteSpace(outputDir))
        {
            Logger.LogWarning("Provided directory was empty and not valid output directory.");
            return DefaultOutputDir;
        }
        try
        {
            // Validate
            Path.GetFullPath(outputDir);
            return outputDir;
        }
        catch (Exception ex)
        {
            Logger.LogException("Provided direcory was not valid output directory", ex);
            return DefaultOutputDir;
        }
    }

    private Stream? CreateOrOverwrite(string filePath)
    {
        string? dir = Path.GetDirectoryName(filePath);
        try
        {
            if (Directory.Exists(filePath))
            {
                Logger.LogInformation("Directory already exists in output path, overwriting.");
                Directory.Delete(filePath);
            }
            if (string.IsNullOrWhiteSpace(dir) is false)
            {
                Directory.CreateDirectory(dir);
            }
            return File.Open(filePath, FileMode.Create);
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Could not create scaled image output file, '{ex}': '{msg}'.",
                ex.GetType().Name, ex.Message);
            return null;
        }
    } 

    /// <summary>
    /// Get multiplyer to satisfy max pixel count.
    /// </summary>
    /// <param name="currentPx"></param>
    /// <param name="maxPx"></param>
    /// <returns>1 if maxPx is 0 or bigger than currentPx, otherwise a float between 0 and 1.</returns>
    private static float GetScaleMultiplyer(int currentPx, int maxPx)
    {
        if (maxPx <= 0)
        {
            return 1;
        }
        if (maxPx > currentPx)
        {
            return 1;
        }
        return (float)maxPx / currentPx;
    }



}
