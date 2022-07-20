using WordListsUI;
using WordListsUI.ListGeneratorPage;
using System.Diagnostics;
using WordListsUI.StartTrainingPage;
using WordListsUI.WordTrainingPage;

namespace WordLists;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(StartTrainingPage), typeof(StartTrainingPage));
		Routing.RegisterRoute(nameof(WordTrainingPage), typeof(WordTrainingPage));
    }
}
