using DesignSphere.Application.Contracts;
//using DesignSphere.Application.Contracts.Services;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using DesignSphere.Infrastructure.Repositories;
using DesignSphere.Infrastructure.Repositries;
//using DesignSphere.Infrastructure.Repositries.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SieveCustom.Services;

namespace DesignSphere.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SQLContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IDoorOpeningRepository, DoorOpeningRepository>();
        services.AddScoped<IContributerRepository, ContributerRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        //services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<ISieveProcessor, SieveProcessor>();

        return services;
    }
}
