using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DesignSphere.Infrastructure.Repositories;

public class DoorOpeningRepository : RepositoryBase<DoorOpening>, IDoorOpeningRepository
{
    private readonly ILogger<ProjectRepository> logger;

    public DoorOpeningRepository(SQLContext dbContext, ILogger<ProjectRepository> logger) : base(dbContext)
    {
        this.logger = logger;
    }
}