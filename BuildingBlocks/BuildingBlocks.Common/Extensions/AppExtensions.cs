using AutoWrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using Serilog;

namespace BuildingBlocks.Common.Extensions;

public static class AppExtensions
{

    public static void AutoWrapperConfigEx(this IApplicationBuilder app)
    {
        app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
        {
            ShowIsErrorFlagForSuccessfulResponse = true,
            UseApiProblemDetailsException = true,
            EnableExceptionLogging = true,
            EnableResponseLogging = false,
            ShouldLogRequestData = false,
            ExcludePaths = [
                new AutoWrapperExcludePath("/img/.*", ExcludeMode.Regex),
                //new AutoWrapperExcludePath("/hub/.*", ExcludeMode.Regex)
                ]
        });
    }

    public static void Seriloging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            // Customize the message template
            //options.MessageTemplate = "Handled {RequestPath}";

            // Emit debug-level events instead of the defaults
            //options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

            // Attach additional properties to the request completion event
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                //diagnosticContext.Set(Constants.IdentityClaims.UserId, httpContext..Connection.cha userContext.UserId);
                //diagnosticContext.Set(Constants.HeaderTypes.Channel, userContext.Channel);
                //diagnosticContext.Set(Constants.HeaderTypes.UserAgent, requestContext.UserAgent);
                //diagnosticContext.Set(Constants.HeaderTypes.CorrelationId, requestContext.CorrelationId);
                diagnosticContext.Set(propertyName: "Requester IP", httpContext.Connection.RemoteIpAddress);
                diagnosticContext.Set("RequestQueryString", httpContext.Request.QueryString);
                //diagnosticContext.Set("RequestBody", httpContext.Request.Body);
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("UserId", httpContext.User.Identity.Name);
            };
        });
    }

    public static void SwaggerConfigEx(this IApplicationBuilder app,
        IConfiguration configuration,
        IEndpointRouteBuilder epbuilder)
    {
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
            {
                if (!httpRequest.Headers.ContainsKey("X-Forwarded-prefix")) return;
                var serverUrl = $"https://{httpRequest.Host}{httpRequest.Headers["X-Forwarded-prefix"]}";
                Console.WriteLine(httpRequest.Headers["X-Forwarded-prefix"]);
                Console.WriteLine(serverUrl);
                swaggerDoc.Servers =
                [
                    new() {Description = "Staging", Url = serverUrl },
                            new() {Description = "Production",Url = configuration.GetValue<string>("AdminConfiguration:Endpoints:Production") },
                            new() {Description = "Development",Url =configuration.GetValue<string>("AdminConfiguration:Endpoints:Development")  }
                ];
            });
        });
        app.UseSwaggerUI(
               options =>
               {
                   var descriptions = epbuilder.DescribeApiVersions();
                   foreach (var description in descriptions)
                   {
                       options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                   }
               });
    }


    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                //logger.LogInformation("Deleting Database", typeof(TContext).Name);
                //context.Database.EnsureDeleted();

                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                var retry = Policy.Handle<NpgsqlException>()
                        .WaitAndRetry(
                            retryCount: 1,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                            onRetry: (exception, retryCount, context) =>
                            {
                                logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                            });

                //if the sql server container is not created on run docker compose this
                //migration can't fail for network related exception. The retry options for DbContext only
                //apply to transient exceptions
               // retry.Execute(() => InvokeSeeder(seeder, context, services));

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            }
        }

        return host;
    }
    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}
