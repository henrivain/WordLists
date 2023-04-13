using System.Collections.ObjectModel;
using WordListsViewModels.Events;
using WordListsViewModels.Helpers;

namespace WordListsViewModels.Interfaces;
public interface IOcrListGeneratorViewModel
{
    object SelectedParser { get; set; }
    bool IsImageCaptureSupported { get; }
    bool IsBusy { get; }
    bool HasValidPairs { get; } 
    int RecogizionConfidence { get; }

    ObservableCollection<ParserInfo> Parsers { get; }
    ObservableCollection<WordPair> Pairs { get; }

    IAsyncRelayCommand NewListFromCamera { get; }
    IAsyncRelayCommand NewListFromFile { get; }
    IAsyncRelayCommand AddListSpanFromCamera { get; }
    IAsyncRelayCommand AddListSpanFromFile { get; }
    
    event ParserErrorEventHandler ParseFailed;
    event TesseractFailedEventHandler RecognizionFailed;
    event TesseractFailedEventHandler NoTextWasRecognized;
}
