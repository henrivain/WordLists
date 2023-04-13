using WordDataAccessLibrary;
using WordListsMauiHelpers.DependencyInjectionExtensions;
using WordListsViewModels.Events;

namespace WordListsUI.WordDataPages.OcrListGeneratorPage;

public partial class OcrListGeneratorPage : ContentPage
{
    public OcrListGeneratorPage(IAbstractFactory<IOcrListGeneratorViewModel> vmFactory)
    {
        InitializeComponent();
        ViewModelFactory = vmFactory;
        BindingContext = vmFactory.Create();
        BindingContextChanged += OcrListGeneratorPage_BindingContextChanged;
    }
    IOcrListGeneratorViewModel ViewModel => (IOcrListGeneratorViewModel)BindingContext;
    IAbstractFactory<IOcrListGeneratorViewModel> ViewModelFactory { get; }

    private void ResetViewModel_Clicked(object sender, EventArgs e)
    {
        BindingContext = ViewModelFactory.Create();
    }

    private void OcrListGeneratorPage_BindingContextChanged(object? sender, EventArgs e)
    {
        ViewModel.ParseFailed += ViewModel_ParseFailed;
        ViewModel.RecognizionFailed += ViewModel_RecognizionFailed;
        ViewModel.NoTextWasRecognized += ViewModel_NoTextWasRecognized;
    }

    private void ViewModel_NoTextWasRecognized(object sender, TesseractFailedEventArgs e)
    {
        throw new NotImplementedException();
    }


    private void ViewModel_RecognizionFailed(object sender, TesseractFailedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ViewModel_ParseFailed(object sender, string error)
    {
        throw new NotImplementedException();
    }

    private void ProceedToEdit_Clicked(object sender, EventArgs e)
    {

    }
}