namespace WordListsUI.WordDataPages.OcrListGeneratorPage;

/// <summary>
/// This page is implementation for the <see cref="BaseOcrListGeneratorPage"/> for small screen devices.
/// To navigate to this page, use <see cref="BaseOcrListGeneratorPage"/> in route.
/// </summary>
public partial class PhoneOcrListGeneratorPage : BaseOcrListGeneratorPage
{
    public PhoneOcrListGeneratorPage(IOcrListGeneratorViewModel viewModel, ILogger<ContentPage> logger)
        : base(viewModel, logger)
    {
        InitializeComponent();
    }
}