using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SonicShare.WebServer;

public class WebAppHostProgram
{
    public static WebApplication CreateWebApp(int httpPort, int httpsPort, string applicationName, MessageDispatcher messageDispatcher, CallbackLoggerProvider loggerProvider)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ApplicationName = applicationName
        });
        builder.WebHost.ConfigureKestrel((context, serverOptions) =>
        {
            serverOptions.Listen(IPAddress.Loopback, httpPort);
            serverOptions.Listen(IPAddress.Loopback, httpsPort, listenOptions =>
            {
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
            });
        });


#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Logging.AddProvider(loggerProvider);
        builder.Logging.SetMinimumLevel(LogLevel.Information);

        builder.Services.AddSingleton(messageDispatcher);

        builder.Services.AddControllers();

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.MapControllers();

        app.MapGet("/", () =>
        {
            return "Hello from Android";
        });

        return app;
    }
}
