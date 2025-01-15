using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using DesignSphere.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SieveCustom.Services;


namespace DesignSphere.Infrastructure.Repositries;

public class ContributerRepository : RepositoryBase<ProjectContributer>, IContributerRepository
{
    private readonly ILogger<ContributerRepository> _logger;

    public ContributerRepository(SQLContext dbContext, ILogger<ContributerRepository> logger, ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
    {
        _logger = logger;
    }


    public async Task<bool> getContributerByProject(Ulid contactId,Ulid projectId)
    {
        var contributer = await dbContext.ProjectContributers
            .FirstOrDefaultAsync(c => c.ContactId == contactId && c.ProjectId == projectId && c.IsContributerRemoved == false);

        return contributer != null;
    }
}
