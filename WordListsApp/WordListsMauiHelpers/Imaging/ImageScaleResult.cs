namespace WordListsMauiHelpers.Imaging;
public readonly struct ImageScaleResult
{
    /// <summary>
    /// Status of image scaling process.
    /// </summary>
    public required ImageStatus Success { get; init; }

    /// <summary>
    /// Path to image.
    /// <para/>Returns path to scaled image if success.
    /// <para/>Returns path to original image if not success.
    /// <para/>Returns null if path null.
    /// </summary>
    public required string? ImagePath { get; init; }

    /// <summary>
    /// Short message representing status, for example error.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Height of scaled image, if success.
    /// </summary>
    public int? Height { get; init; }

    /// <summary>
    /// Width of scaled image, if success.
    /// </summary>
    public int? Width { get; init; }
    
}
