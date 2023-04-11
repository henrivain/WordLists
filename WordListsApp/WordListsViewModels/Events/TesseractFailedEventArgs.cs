using TesseractOcrMaui.Results;

namespace WordListsViewModels.Events;

public class TesseractFailedEventArgs
{
    public string? Message { get; init; }

    public string? ImagePath { get; init; }
    public RecognizionResult? Output { get; init; }
}