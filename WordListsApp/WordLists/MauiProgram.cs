using WordListsMauiHelpers.Factories;
using WordListsUI.ListGeneratorPage;
using WordListsUI.StartTrainingPage;
using WordListsUI.WordTrainingPage;
using WordListsViewModels;

namespace WordLists;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		// injecting appshell will make app buggy and starts to change visual element visibility
		builder.Services.AddTransient<ListGeneratorPage>();
		builder.Services.AddTransient<WordTrainingPage>();
		builder.Services.AddTransient<StartTrainingPage>();
		builder.Services.AddSingleton<IWordTrainingViewModel, WordTrainingViewModel>();
		builder.Services.AddSingleton<IStartTrainingViewModel, StartTrainingViewModel>();
		builder.Services.AddAbstractFactory<IListGeneratorViewModel, ListGeneratorViewModel>();

		return builder.Build();
	}
}
