using WordListsViewModels.Events;
namespace WordListsUI.AppInfoPage;

public partial class AppInfoPage : ContentPage
{
	public AppInfoPage(IAppInfoViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
        model.LogsCopied += Model_LogsCopied;
        model.ShowLogFileFailed += Model_ShowLogFileFailed;
	}

    private async void Model_ShowLogFileFailed(object sender, InvalidDataEventArgs<string[]> e)
    {
        await DisplayAlert("Avaaminen epäonnistui!", $"Lokitiedostokansion avaaminen epäonnistui. Syy: '{e.Message}'.", "OK");
    }

    private async void Model_LogsCopied(object sender, LogsCopiedEventArgs e)
    {
        if (e.Success)
        {
            await DisplayAlert("Lokitiedostot kopioitu!",
                $"{e.FilesValid} lokitieodostoa kopioitu onnistuneesti polkuun: \n {e.OutputDirectory}",
                "OK");
            return;
        }
        int filesTotal = e.FilesValid + e.FilesFailed;
        await DisplayAlert("Tiedostojen kopiointi epäonnistui!",
            $"{e.FilesFailed}/{filesTotal} lokitiedostoa ei pystytty kopioimaan. \nSyy:\n {e.Message}", 
            "OK");
    }

    public IAppInfoViewModel Model => (IAppInfoViewModel)BindingContext;
}