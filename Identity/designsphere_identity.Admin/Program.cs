using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Skoruba.IdentityServer4.Admin.EntityFramework.Configuration.Configuration;
using Skoruba.IdentityServer4.Shared.Configuration.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.DbContexts;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;
using designsphere_identity.Admin.EntityFramework.Shared.Helpers;

namespace designsphere_identity.Admin;

public class Program
{
    private const string SeedArgs = "/seed";
    private const string MigrateOnlyArgs = "/migrateonly";

    public static async Task Main(string[] args)
    {
        var configuration = GetConfiguration(args);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            DockerHelpers.ApplyDockerConfiguration(configuration);

            var host = CreateHostBuilder(args).Build();

            var migrationComplete = await ApplyDbMigrationsWithDataSeedAsync(args, configuration, host);
            if (args.Any(x => x == MigrateOnlyArgs))
            {
                await host.StopAsync();
                if (!migrationComplete)
                {
                    Environment.ExitCode = -1;
                }

                return;
            }
            await host.RunAsync();
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

    private static async Task<bool> ApplyDbMigrationsWithDataSeedAsync(string[] args, IConfiguration configuration, IHost host)
    {
        var applyDbMigrationWithDataSeedFromProgramArguments = args.Any(x => x == SeedArgs);
        applyDbMigrationWithDataSeedFromProgramArguments = true;
        if (applyDbMigrationWithDataSeedFromProgramArguments) args = args.Except(new[] { SeedArgs }).ToArray();

        var seedConfiguration = configuration.GetSection(nameof(SeedConfiguration)).Get<SeedConfiguration>();
        var databaseMigrationsConfiguration = configuration.GetSection(nameof(DatabaseMigrationsConfiguration))
            .Get<DatabaseMigrationsConfiguration>();

        return await DbMigrationHelpers
            .ApplyDbMigrationsWithDataSeedAsync<IdentityServerConfigurationDbContext, AdminIdentityDbContext,
                IdentityServerPersistedGrantDbContext, AdminLogDbContext, AdminAuditLogDbContext,
                IdentityServerDataProtectionDbContext, UserIdentity, UserIdentityRole>(host,
                applyDbMigrationWithDataSeedFromProgramArguments, seedConfiguration, databaseMigrationsConfiguration);
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
                 configApp.AddJsonFile("identitydata.json", optional: true, reloadOnChange: true);
                 configApp.AddJsonFile("identityserverdata.json", optional: true, reloadOnChange: true);

                 var env = hostContext.HostingEnvironment;

                 configApp.AddJsonFile($"serilog.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                 configApp.AddJsonFile($"identitydata.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                 configApp.AddJsonFile($"identityserverdata.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

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
            }).UseSerilog((context, services, configuration) =>
              configuration.ReadFrom.Configuration(context.Configuration)
                  .ReadFrom.Services(services)
                  .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                  .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                  .Enrich.WithProperty("ApplicationVersion", Assembly.GetExecutingAssembly().GetName().Version)
                  .Enrich.FromLogContext()
                  .WriteTo.Conditional(x => !context.HostingEnvironment.IsProduction(), x => x.Console().WriteTo.Console())
            );
}
