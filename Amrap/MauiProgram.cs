using Microsoft.Extensions.Logging;
using Amrap.Infrastructure.Db;
using Amrap.Core;

namespace Amrap;

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
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
        // builder.Services.AddMauiBlazorWebViewDeveloperTools instead of above?
        builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<DatabaseHandler>();

		builder.Services.AddSingleton<WorkoutPlanRetriever>();
		builder.Services.AddSingleton<CompletedExerciseSaver>();
		builder.Services.AddSingleton<CompletedExerciseReader>();

		return builder.Build();
	}
}
