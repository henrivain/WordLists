using System.Diagnostics;
using WordDataAccessLibrary;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.Generators;
using WordListsViewModels;

namespace WordListsUI.ListGeneratorPage;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListGeneratorPage : ContentPage
{
	public ListGeneratorPage(IListGeneratorViewModel model)
	{
        BindingContext = model;
		InitializeComponent();
	}

    public IListGeneratorViewModel ViewModel => (IListGeneratorViewModel)BindingContext;
}