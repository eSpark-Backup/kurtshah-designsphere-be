using DesignSphere.Application.Contracts;
using DesignSphere.Infrastructure.Persistence;
using DesignSphere.Infrastructure.Repositories;
using Location.Application.Contracts.Persistence;
using Location.Infrastructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesignSphere.Infrastructure;

public class Class1
{

}
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SQLContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped(typeof(IGpsRepository), typeof(GpsRepository));

        return services;
    }
}
