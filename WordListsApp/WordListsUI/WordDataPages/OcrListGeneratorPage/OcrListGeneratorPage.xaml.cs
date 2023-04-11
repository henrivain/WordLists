using WordListsMauiHelpers.DependencyInjectionExtensions;

namespace WordListsUI.WordDataPages.OcrListGeneratorPage;

public partial class OcrListGeneratorPage : ContentPage
{
	public OcrListGeneratorPage(IAbstractFactory<IOcrListGeneratorViewModel> vmFactory)
	{
		InitializeComponent();
        ViewModelFactory = vmFactory;
        BindingContext = vmFactory.Create();
    }

    IOcrListGeneratorViewModel ViewModel => (IOcrListGeneratorViewModel)BindingContext;
    IAbstractFactory<IOcrListGeneratorViewModel> ViewModelFactory { get; }
}