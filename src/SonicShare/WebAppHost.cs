using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using SonicShare.WebServer;

namespace SonicShare;

public class WebAppHost : IAsyncDisposable
{
    private readonly ILogger<WebAppHost> logger;
    private readonly MessageDispatcher dispatcher;
    private readonly CallbackLoggerProvider loggerProvider;
    private WebApplication? app;

    public WebAppHost(ILogger<WebAppHost> logger, MessageDispatcher messageDispatcher, CallbackLoggerProvider loggerProvider)
    {
        this.logger = logger;
        dispatcher = messageDispatcher;
        this.loggerProvider = loggerProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting web app.");

        try
        {

            app = WebAppHostProgram.CreateWebApp(
                httpPort: 5000,
                httpsPort: 5001,
                "SonicShare",
                dispatcher,
                loggerProvider);

            await app.StartAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error from web app startup.");
        }
    }

    public  ValueTask StopAsync()
    {
        return CleanupAsync();
    }
    public ValueTask DisposeAsync()
    {
        return CleanupAsync();
    }

    public async ValueTask CleanupAsync()
    {
        if (app == null)
            return;

        logger.LogInformation("Stopping web app.");
        await app.StopAsync();
        await app.DisposeAsync();
    }
}