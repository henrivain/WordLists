using System.Diagnostics;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.Generators;
using WordListsViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;

namespace WordListsUI.ListGeneratorPage;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListGeneratorPage : ContentPage
{
	public ListGeneratorPage(IListGeneratorViewModel model)
	{
        BindingContext = model;
		InitializeComponent();
		hh.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetAccessKey("D");
	}

    public IListGeneratorViewModel ViewModel => (IListGeneratorViewModel)BindingContext;
}