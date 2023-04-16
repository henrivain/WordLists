namespace WordListsMauiHelpers.Imaging;
public readonly struct ImageScaleResult
{
    public required ImageStatus Success { get; init; }
    public required string? ImagePath { get; init; }
    public string? Message { get; init; }
    public int? Height { get; init; }
    public int? Width { get; init; }
    
}
