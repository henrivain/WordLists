using System.Collections.ObjectModel;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Interfaces;
public interface IOcrListGeneratorViewModel
{
    object SelectedParser { get; set; }
    bool IsImageCaptureSupported { get; }
    bool IsBusy { get; }
    bool CanParse { get; }
    string RecognizedText { get; }
    int RecogizionConfidence { get; }

    ObservableCollection<ParserInfo> Parsers { get; }

    IAsyncRelayCommand CaptureAndRecognizeCommand { get; }
    IAsyncRelayCommand SelectAndRecognizeCommand { get; }
    IAsyncRelayCommand ParseAndShowCommand { get; }
    
    public event ParserErrorEventHandler ParseFailed;
    public event TesseractFailedEventHandler RecognizionFailed;
    public event WordPairGenSuccessEventHandler ParseSucceeded;
}
