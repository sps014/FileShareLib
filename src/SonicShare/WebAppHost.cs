using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using SonicShare.WebServer;

namespace SonicShare;

public class WebAppHost : IAsyncDisposable
{
    private readonly ILogger<WebAppHost> _logger;
    private readonly MessageDispatcher dispatcher;
    private readonly CallbackLoggerProvider _loggerProvider;
    private WebApplication? _app;

    public WebAppHost(ILogger<WebAppHost> logger, MessageDispatcher messageDispatcher, CallbackLoggerProvider loggerProvider)
    {
        _logger = logger;
        dispatcher = messageDispatcher;
        _loggerProvider = loggerProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting web app.");

        try
        {

            _app = WebAppHostProgram.CreateWebApp(
                httpPort: 5000,
                httpsPort: 5001,
                "SonicShare",
                dispatcher,
                _loggerProvider);

            await _app.StartAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error from web app startup.");
        }
    }


    public async ValueTask DisposeAsync()
    {
        _logger.LogInformation("Stopping web app.");

        if (_app != null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }
}