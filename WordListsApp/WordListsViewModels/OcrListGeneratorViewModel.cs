using System.Collections.ObjectModel;
using TesseractOcrMaui;
using TesseractOcrMaui.Extensions;
using TesseractOcrMaui.Results;
using WordDataAccessLibrary.Generators;
using WordListsMauiHelpers.Settings;
using WordListsViewModels.Helpers;

namespace WordListsViewModels;
public partial class OcrListGeneratorViewModel : ObservableObject, IOcrListGeneratorViewModel
{
    public OcrListGeneratorViewModel(
        ITesseract tesseract, 
        IMediaPicker mediaPicker, 
        IFilePicker filePicker,
        IEnumerable<IWordPairParser> parsers,
        ILogger<OcrListGeneratorViewModel> logger,
        ISettings settings
        )
    {
        Tesseract = tesseract;
        MediaPicker = mediaPicker;
        FilePicker = filePicker;
        Logger = logger;
        Settings = settings;
        Parsers = new(parsers.Where(x => x is IOcrWordPairParser).ToParserInfos());
        if (Parsers.Count < 1)
        {
            throw new ArgumentException("At least one ocr parser must be defined.", nameof(parsers));
        }

        _selectedParser = Parsers.FirstOrGivenDefault(x => x.Name == (Settings.DefaultOcrParserName ?? string.Empty), Parsers[0]);
        _isImageCaptureSupported = MediaPicker.IsCaptureSupported;
    }

    static string[] SupportedImageTypes { get; } = new[] { "png", "jpeg", "jpg" };
    HashSet<string> SupportedFileExtensions { get; } = SupportedImageTypes.Select(x => $".{x}").ToHashSet();
    Dictionary<DevicePlatform, IEnumerable<string>> ImageFilePickerFileTypes { get; } = new()
    {
        [DevicePlatform.Android] = SupportedImageTypes.Select(x => $"image/{x}"),
        [DevicePlatform.WinUI] = SupportedImageTypes,
    };

    ITesseract Tesseract { get; }
    IMediaPicker MediaPicker { get; }
    IFilePicker FilePicker { get; }
    ILogger<IOcrListGeneratorViewModel> Logger { get; }
    ISettings Settings { get; }

    [ObservableProperty]
    bool _isImageCaptureSupported;

    [ObservableProperty]
    object _selectedParser;
    
    public ObservableCollection<ParserInfo> Parsers { get; private set; }
    public IAsyncRelayCommand CaptureAndRecognizeCommand => new AsyncRelayCommand(async () =>
    {
        FileResult fileResult = await MediaPicker.CapturePhotoAsync();
        if (fileResult is null)
        {
            Logger.LogInformation("User did not take photo");
            return;
        }
        await RecognizeTextAsync(fileResult.FullPath);
    });
    public IAsyncRelayCommand SelectAndRecognizeCommand => new AsyncRelayCommand(async () =>
    {
        PickOptions options = new()
        {
            PickerTitle = "Valitse kuva.",
            FileTypes = new(ImageFilePickerFileTypes)
        };

        FileResult? fileResult = await FilePicker.PickAsync(options);
        if (fileResult is null)
        {
            Logger.LogInformation("User did not choose file.");
            return;
        }
        await RecognizeTextAsync(fileResult.FullPath);
    });

    public string RecognizedText { get; private set; } = string.Empty;
    public int RecogizionConfidence { get; private set; } = 0;

    private async Task RecognizeTextAsync(string? filePath)
    {
        Logger.LogInformation("Recongize text from image '{path}'", filePath);
        if (File.Exists(filePath) is false)
        {
            Logger.LogWarning("Cannot recognize, image file does not exist.");
            return;
        }
        if (SupportedFileExtensions.Contains(Path.GetExtension(filePath)))
        {
            Logger.LogWarning("Cannot recognize, invalid image format '{format}', must be one of '{extension}'.", 
                Path.GetExtension(filePath), string.Join(", ", SupportedFileExtensions));
            return;
        }
        RecognizionResult textResult = await Tesseract.RecognizeTextAsync(filePath);
        if (textResult.NotSuccess())
        {
            Logger.LogWarning("Failed to recognize text in image: '{msg}'.", textResult.Message);
            RecognizedText = string.Empty;
            RecogizionConfidence = 0;
            return;
        }
        RecognizedText = textResult.RecognisedText ?? string.Empty;
        RecogizionConfidence = textResult.Confidence > 0 ? (int)Math.Ceiling(textResult.Confidence) : 0;
    }
}
