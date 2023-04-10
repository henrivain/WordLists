using TesseractOcrMaui;

namespace WordListsViewModels;
public class OcrListGeneratorViewModel : ObservableObject, IOcrListGeneratorViewModel
{
    public OcrListGeneratorViewModel(ITesseract tesseract)
    {
        Tesseract = tesseract;
    }

    public ITesseract Tesseract { get; }




}
