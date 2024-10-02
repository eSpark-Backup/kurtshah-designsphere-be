using Microsoft.Extensions.Hosting;
using Serilog.Filters;
using Serilog;
using System.Reflection;

namespace BuildingBlocks.Common.Infrastructure;
public static class SeriLogger
{
    public static Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> Configure =>
       (context, services, configuration) =>
       {
           configuration.ReadFrom.Configuration(context.Configuration)
               .ReadFrom.Services(services)
                .Filter.ByExcluding(Matching.WithProperty("RequestPath", "/health/readiness"))
                .Filter.ByExcluding(Matching.WithProperty("RequestPath", "/health/liveness"))
               .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
               .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
               .Enrich.WithProperty("ApplicationVersion", Assembly.GetExecutingAssembly().GetName().Version)
               .Enrich.WithProperty("MachineName", Environment.MachineName)
               .Enrich.FromLogContext()
               .WriteTo.Conditional(x => !context.HostingEnvironment.IsProduction() && !context.HostingEnvironment.IsStaging(), x => x.Console().WriteTo.Debug());
       };
}
