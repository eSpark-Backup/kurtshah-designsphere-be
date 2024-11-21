using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace DesignSphere.Infrastructure.Repositories;

public class GpsRepository : RepositoryBase<Project>, IGpsRepository
{
    private readonly ILogger<GpsRepository> logger;

    public GpsRepository(SQLContext dbContext, ILogger<GpsRepository> logger) : base(dbContext)
    {
        this.logger = logger;
    }
}
