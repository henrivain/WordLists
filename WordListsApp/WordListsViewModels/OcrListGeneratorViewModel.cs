using System.Collections.ObjectModel;
using System.ComponentModel;
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
        PropertyChanged += UpdateSettingValue;
    }

    static string[] SupportedImageTypes { get; } = new[] { "png", "jpeg", "jpg" };
    HashSet<string> SupportedFileExtensions { get; } = SupportedImageTypes.Select(x => $".{x}").ToHashSet();
    Dictionary<DevicePlatform, IEnumerable<string>> ImageFilePickerFileTypes { get; } = new()
    {
        [DevicePlatform.Android] = SupportedImageTypes.Select(x => $"image/{x}"),
        [DevicePlatform.WinUI] = SupportedImageTypes,
    };

    const int TesseractTimeoutMs = 30000;

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

    [ObservableProperty]
    bool _isRefreshing = false;

    [ObservableProperty]
    bool _hasValidPairs = false;

    [ObservableProperty]
    int _recogizionConfidence = 0;

    // Uses Clear() and Add() methods instead of setters
    public ObservableCollection<WordPair> Pairs { get; } = new();
    public ObservableCollection<ParserInfo> Parsers { get; }


    public IRelayCommand ClearWordsCommand => new RelayCommand(() =>
    {
        IsRefreshing = true;
        Pairs.Clear();
        HasValidPairs = false;
        IsRefreshing = false;
    });
    public IAsyncRelayCommand AddListSpanFromCameraCommand => new AsyncRelayCommand(async () =>
    {
        IsImageCaptureSupported = MediaPicker.IsCaptureSupported;
        IsBusy = true;
        OcrResult? result = await CaptureImageAndRecognize();
        if (result.HasValue)
        {
            RecogizionConfidence = result.Value.Confidence;
            await ParseAndAdd(result.Value.Text);
        }
        IsBusy = false;
    });
    public IAsyncRelayCommand AddListSpanFromFileCommand => new AsyncRelayCommand(async () =>
    {
        IsBusy = true;
        OcrResult? result = await PickImageAndRecognize();
        if (result.HasValue)
        {
            RecogizionConfidence = result.Value.Confidence;
            await ParseAndAdd(result.Value.Text);
        }
        IsBusy = false;
    });


    public event ParserErrorEventHandler? ParseFailed;
    public event TesseractFailedEventHandler? RecognizionFailed;
    public event TesseractFailedEventHandler? NoTextWasRecognized;

    private record struct OcrResult(string Text, int Confidence);

    /// <summary>
    /// Recognizes ocrText from image file. 
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>OcrResult if successful, otherwise rises RecognizionFailed event and returns null.</returns>
    private async Task<OcrResult?> RecognizeTextAsync(string? filePath)
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
            return null;
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
            return null;
        }

        Task<RecognizionResult> recognizionTask = Tesseract.RecognizeTextAsync(filePath);
        if (await Task.WhenAny(recognizionTask, Task.Delay(TesseractTimeoutMs)) != recognizionTask)
        {
            Logger.LogWarning("Cannot recognize, timeout.");
            RecognizionFailed?.Invoke(this, new()
            {
                ImagePath = filePath,
                Message = "Timeout, took too long to process."
            });
            return null;
        }
        RecognizionResult result = await recognizionTask;
        if (result.NotSuccess())
        {
            Logger.LogWarning("Failed to recognize text in image: '{msg}'.", result.Message);
            RecognizionFailed?.Invoke(this, new()
            {
                ImagePath = filePath,
                Message = "Ocr failed.",
                Output = result
            });
            return null;
        }
        string text = result.RecognisedText ?? string.Empty;
        Logger.LogInformation("Ocr found '{count}' characters with confidence '{conf}'.",
            text.Length, result.Confidence);

        if (result.RecognisedText?.Length <= 0)
        {
            Logger.LogInformation("No text recognized in given image.");
            NoTextWasRecognized?.Invoke(this, new TesseractFailedEventArgs
            {
                ImagePath = filePath,
                Message = "No text recognized, image might not have any text.",
                Output = result
            });
            return null;
        }

        return new OcrResult
        {
            Text = text,
            Confidence = result.Confidence > 0 ? (int)Math.Ceiling(result.Confidence * 100) : 0
        };

    }

    /// <summary>
    /// Let user pick file and recognize it. Handles errors.
    /// </summary>
    /// <returns>OcrResult if success, otherwise null.</returns>
    private async Task<OcrResult?> PickImageAndRecognize()
    {
        Logger.LogInformation("Pick image and recognize text.");
        PickOptions options = new()
        {
            PickerTitle = "Valitse kuva...",
            FileTypes = new(ImageFilePickerFileTypes)
        };

        FileResult? fileResult = await FilePicker.PickAsync(options);
        if (fileResult is null)
        {
            Logger.LogInformation("User did not choose file.");
            return null;
        }
        return await RecognizeTextAsync(fileResult.FullPath);
    }

    /// <summary>
    /// Let user capture image and recognize it. Handles errors.
    /// </summary>
    /// <returns>OcrResult if success, otherwise null.</returns>
    private async Task<OcrResult?> CaptureImageAndRecognize()
    {
        Logger.LogInformation("Capture image and recognize text.");
        FileResult fileResult = await MediaPicker.CapturePhotoAsync();
        if (fileResult is null)
        {
            Logger.LogInformation("User did not take photo.");
            return null;
        }
        return await RecognizeTextAsync(fileResult.FullPath);
    }

    /// <summary>
    /// Parse string to word pairs and add them to RightWords and LeftWords.
    /// <para/>Clears output properties before adding new values if clearsOutput is true.
    /// </summary>
    /// <param name="ocrText"></param>
    /// <param name="clearsOutput"></param>
    /// <returns>awaitable Task</returns>
    private async Task ParseAndAdd(string ocrText)
    {
        Logger.LogInformation("Parse ocr string to word pairs.");
        if (string.IsNullOrWhiteSpace(ocrText))
        {
            Logger.LogWarning("{cls} Parser was given empty string.", nameof(OcrListGeneratorViewModel));
            ParseFailed?.Invoke(this, "No text to parse.");
            return;
        }

        // Select parser
        IWordPairParser? parser = null;
        if (SelectedParser is ParserInfo parserInfo)
        {
            parser = parserInfo.Parser;
        }
        parser ??= Parsers[0].Parser;

        // Parse
        var pairs = await Task.Run(() =>
        {
            try
            {
                return parser.GetList(ocrText);
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
            Logger.LogError("Failed to parse ocr text, parser failed.");
            ParseFailed?.Invoke(this, "Parser failed, see logs.");
            return;
        }
        Logger.LogInformation("Parsed ocr text to '{count}' word pairs.", pairs.Count);

        foreach (var pair in pairs)
        {
            Pairs.Add(pair);
        }
        HasValidPairs = true;
    }

    private void UpdateSettingValue(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(SelectedParser):
                if (SelectedParser is ParserInfo info && Settings.DefaultOcrParserName != info.Name)
                {
                    Settings.DefaultOcrParserName = info.Name;
                }
                break;
        }
    }
}
