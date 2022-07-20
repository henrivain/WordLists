using Microsoft.Maui.Controls.Compatibility.Hosting;
using WordListsUI.ListGeneratorPage;
using WordListsUI.WordTrainingPage;
using WordListsViewModels;
using WordListsMauiHelpers.Factories;
using WordListsUI.StartTrainingPage;

namespace WordLists;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCompatibility()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		// injectin appshell will make app buggy and starts to change visual element visibility
		builder.Services.AddTransient<ListGeneratorPage>();
		builder.Services.AddTransient<WordTrainingPage>();
		builder.Services.AddTransient<StartTrainingPage>();
		//builder.Services.AddTransient<IListGeneratorViewModel, ListGeneratorViewModel>();
		builder.Services.AddSingleton<IWordTrainingViewModel, WordTrainingViewModel>();
		builder.Services.AddSingleton<IStartTrainingViewModel, StartTrainingViewModel>();

		builder.Services.AddAbstractFactory<IListGeneratorViewModel, ListGeneratorViewModel>();

		return builder.Build();
	}
}
