using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Skoruba.IdentityServer4.Shared.Configuration.Helpers;
using System;
using System.IO;
using System.Reflection;

namespace designsphere_identity.STS.Identity;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = GetConfiguration(args);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        try
        {
            DockerHelpers.ApplyDockerConfiguration(configuration);

            CreateHostBuilder(args)

                .Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IConfiguration GetConfiguration(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var isDevelopment = environment == Environments.Development;

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true);

        if (isDevelopment)
        {
            configurationBuilder.AddUserSecrets<Startup>(true);
        }

        var configuration = configurationBuilder.Build();

        configuration.AddAzureKeyVaultConfiguration(configurationBuilder);

        configurationBuilder.AddCommandLine(args);
        configurationBuilder.AddEnvironmentVariables();

        return configurationBuilder.Build();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((hostContext, configApp) =>
             {
                 var configurationRoot = configApp.Build();

                 configApp.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);

                 var env = hostContext.HostingEnvironment;

                 configApp.AddJsonFile($"serilog.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                 if (env.IsDevelopment())
                 {
                     configApp.AddUserSecrets<Startup>(true);
                 }

                 configurationRoot.AddAzureKeyVaultConfiguration(configApp);

                 configApp.AddEnvironmentVariables();
                 configApp.AddCommandLine(args);
             })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureServices((hostContext, services) =>
            {
                // Add CORS policy
                services.AddCors(options =>
                {
                    options.AddPolicy("AllowLocalhost", builder =>
                    {
                        builder.WithOrigins("https://localhost:3000", "http://localhost:8080") // Your allowed origin
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                });
            })
            .UseSerilog((context, services, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("ApplicationVersion", Assembly.GetExecutingAssembly().GetName().Version)
                .Enrich.FromLogContext()
                .WriteTo.Conditional(x => !context.HostingEnvironment.IsProduction(), x => x.Console().WriteTo.Console())
            );
}
