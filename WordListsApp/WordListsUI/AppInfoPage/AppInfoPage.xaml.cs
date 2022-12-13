namespace WordListsUI.AppInfoPage;

public partial class AppInfoPage : ContentPage
{
	public AppInfoPage(IAppInfoViewModel model)
	{
		BindingContext = model;
		InitializeComponent();
        model.LogsCopied += Model_LogsCopied;
	}

    private async void Model_LogsCopied(object sender, WordListsViewModels.Events.LogsCopiedEventArgs e)
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
            $"{e.FilesFailed}/{filesTotal} lokitiedostoa ei pystytty kopioimaan. \nSyy: {e.Message}", 
            "OK");
    }

    public IAppInfoViewModel Model => (IAppInfoViewModel)BindingContext;
}