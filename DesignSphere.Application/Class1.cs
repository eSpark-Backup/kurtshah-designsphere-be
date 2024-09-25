using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesignSphere.Application;

public class Class1
{

}
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}