using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

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

        var assembly = typeof(WebAppHostProgram).Assembly;


#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost", builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000", "http://localhost:5173") // Add your localhost ports here
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // If you need cookies/auth
            });
        });

        builder.Logging.AddProvider(loggerProvider);
        builder.Logging.SetMinimumLevel(LogLevel.Information);

        builder.Services.AddSingleton(messageDispatcher);
        builder.Services.AddSingleton(FileManager.Current);

        builder.Services.AddControllers().AddApplicationPart(assembly);

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        var embeddedProvider = new ManifestEmbeddedFileProvider(assembly, "wwwroot");

        app.UseDefaultFiles(new DefaultFilesOptions
        {
            FileProvider = embeddedProvider
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = embeddedProvider
        });

        app.UseCors("AllowLocalhost");

        app.UseDefaultFiles(); // Serves index.html by default
        app.UseStaticFiles();

        app.UseRouting();
        app.MapControllers();

        app.MapFallbackToFile("/SonicShare.WebServer/index.html"); // Handles client-side routes

        return app;
    }
}
