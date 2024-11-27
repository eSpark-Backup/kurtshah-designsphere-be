using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DesignSphere.Application;


public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        return services
            .AddAutoMapper(assembly)
            .AddValidatorsFromAssembly(assembly) 
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    }
}