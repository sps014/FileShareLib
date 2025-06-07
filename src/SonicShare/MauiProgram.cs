using Microsoft.Extensions.Logging;
using SonicShare.WebServer;

namespace SonicShare
{
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
    		builder.Logging.AddDebug();
#endif


            builder.Services.AddSingleton<MessageDispatcher>();
            builder.Services.AddSingleton<CallbackLoggerProvider>();
            builder.Services.AddSingleton<WebAppHost>();
            builder.Services.AddSingleton(FileManager.Current);

            return builder.Build();
        }
    }
}
