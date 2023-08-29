using Amrap.Core;
using Amrap.Core.Infrastructure;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

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
            })
            .ConfigureEssentials(essentials =>
            {
                essentials.UseVersionTracking();
            });
        builder.UseMauiCommunityToolkit();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        // builder.Services.AddMauiBlazorWebViewDeveloperTools instead of above?
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<DatabaseHandler>();

        builder.Services.AddSingleton<CompletedExerciseReader>();

        return builder.Build();
    }
}