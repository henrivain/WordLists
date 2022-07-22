using WordListsUI;
using WordListsUI.ListGeneratorPage;
using System.Diagnostics;
using WordListsUI.StartTrainingPage;
using WordListsUI.WordTrainingPage;
using WordListsUI.WordCollectionHandlingPage;

namespace WordLists;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute($"Training/{nameof(StartTrainingPage)}", typeof(StartTrainingPage));
		Routing.RegisterRoute($"Training/{nameof(WordTrainingPage)}", typeof(WordTrainingPage));
		Routing.RegisterRoute(nameof(WordCollectionHandlingPage), typeof(WordCollectionHandlingPage));
    }
}
