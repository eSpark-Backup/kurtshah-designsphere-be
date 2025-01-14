using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using DesignSphere.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using SieveCustom.Services;


namespace DesignSphere.Infrastructure.Repositries;

public class ProjectRoleRepository : RepositoryBase<ProjectRole>, IProjectRoleRepository
{
    private readonly ILogger<ProjectRoleRepository> _logger;

    public ProjectRoleRepository(SQLContext dbContext, ILogger<ProjectRoleRepository> logger, ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
    {
        _logger = logger;
    }
}
