using System.Collections.ObjectModel;
using TesseractOcrMaui;
using TesseractOcrMaui.Extensions;
using TesseractOcrMaui.Results;
using WordDataAccessLibrary.Generators;
using WordListsMauiHelpers.Settings;
using WordListsViewModels.Events;
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
            throw new ArgumentException("At least one ocr parser must be defined.",
                nameof(parsers));
        }

        _selectedParser = Parsers.FirstOrGivenDefault(
            x => x.Name == (Settings.DefaultOcrParserName ?? string.Empty), Parsers[0]);
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
    object _selectedParser;

    [ObservableProperty]
    bool _isImageCaptureSupported;

    [ObservableProperty]
    bool _isBusy = false;

    public bool CanParse => string.IsNullOrWhiteSpace(RecognizedText) is false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanParse))]
    string _recognizedText = string.Empty;

    [ObservableProperty]
    int _recogizionConfidence = 0;


    public ObservableCollection<ParserInfo> Parsers { get; private set; }
    public IAsyncRelayCommand CaptureAndRecognizeCommand => new AsyncRelayCommand(async () =>
    {

        FileResult fileResult = await MediaPicker.CapturePhotoAsync();
        if (fileResult is null)
        {
            Logger.LogInformation("User did not take photo.");
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
    public IAsyncRelayCommand ParseAndShowCommand => new AsyncRelayCommand(async () =>
    {
        Logger.LogInformation("Parse ocr string to word pairs.");
        if (string.IsNullOrWhiteSpace(RecognizedText))
        {
            Logger.LogWarning("{cls} No text to parse.", nameof(OcrListGeneratorViewModel));
            ParseFailed?.Invoke(this, "No text to parse.");
            return;
        }
        IWordPairParser? parser = null;
        if (SelectedParser is ParserInfo parserInfo)
        {
            parser = parserInfo.Parser;
        }
        parser ??= Parsers[0].Parser;

        List<WordPair>? pairs = await Task.Run(() =>
        {
            try
            {
                return parser.GetList(RecognizedText);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to parse ocr text, because of '{ex}': '{msg}'",
                    ex.GetType().Name, ex.Message);
                return null;
            }
        });

        if (pairs is null)
        {
            ParseFailed?.Invoke(this, "Parser failed, see logs.");
            return;
        }
        Logger.LogInformation("Parsed '{count}' ocr word pairs.", pairs.Count);
        ParseSucceeded?.Invoke(this, pairs);
    });


    public event ParserErrorEventHandler? ParseFailed;
    public event TesseractFailedEventHandler? RecognizionFailed;
    public event WordPairGenSuccessEventHandler? ParseSucceeded;

    
    private async Task RecognizeTextAsync(string? filePath)
    {
        Logger.LogInformation("Recongize text from image '{path}'", filePath);
        if (File.Exists(filePath) is false)
        {
            Logger.LogWarning("Cannot recognize, image file does not exist.");
            RecognizionFailed?.Invoke(this, new()
            {
                ImagePath = filePath,
                Message = "File does not exist."
            });
            return;
        }
        
        string extension = Path.GetExtension(filePath);
        if (SupportedFileExtensions.Contains(extension) is false)
        {
            string supported = string.Join(", ", SupportedFileExtensions.Select(x => $"'{x}'"));

            Logger.LogWarning("Cannot recognize, invalid image format '{format}', must be one of '{extensions}'.",
                extension, supported);
            RecognizionFailed?.Invoke(this, new()
            {
                ImagePath = filePath,
                Message = $"Invalid file format. Extension must be one of '{supported}', was given '{extension}'."
            });
            return;
        }

        IsBusy = true;
        RecognizionResult result = await Tesseract.RecognizeTextAsync(filePath);
        IsBusy = false;

        if (result.NotSuccess())
        {
            Logger.LogWarning("Failed to recognize text in image: '{msg}'.", result.Message);
            RecognizedText = string.Empty;
            RecogizionConfidence = 0;

            RecognizionFailed?.Invoke(this, new()
            {
                ImagePath = filePath,
                Message = "Ocr failed.",
                Output = result
            });
            return;
        }
        string text = result.RecognisedText ?? string.Empty;

        Logger.LogInformation("Ocr found '{count}' characters with confidence '{conf}'.",
            text.Length, result.Confidence);

        RecognizedText = text;
        RecogizionConfidence = result.Confidence > 0 ? (int)Math.Ceiling(result.Confidence) : 0;
    }
}
