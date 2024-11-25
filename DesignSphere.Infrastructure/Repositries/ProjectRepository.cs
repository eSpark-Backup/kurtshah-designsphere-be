using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace DesignSphere.Infrastructure.Repositories;

public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
{
    private readonly ILogger<ProjectRepository> logger;

    public ProjectRepository(SQLContext dbContext, ILogger<ProjectRepository> logger) : base(dbContext)
    {
        this.logger = logger;
    }
}
