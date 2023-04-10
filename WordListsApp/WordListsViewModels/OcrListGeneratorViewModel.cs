using TesseractOcrMaui;

namespace WordListsViewModels;
public class OcrListGeneratorViewModel : ObservableObject, IOcrListGeneratorViewModel
{
    public OcrListGeneratorViewModel(ITesseract tesseract)
    {
        Tesseract = tesseract;
    }

    public ITesseract Tesseract { get; }

    public IAsyncRelayCommand CaptureAndRecognizeCommand => new AsyncRelayCommand(async () =>
    {

    });

    public IAsyncRelayCommand SelectAndRecognizeCommand => new AsyncRelayCommand(async () =>
    {

    });


}
