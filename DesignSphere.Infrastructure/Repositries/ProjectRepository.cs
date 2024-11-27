using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SieveCustom.Models;
using SieveCustom.Services;

namespace DesignSphere.Infrastructure.Repositories;


public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
{
    private readonly ILogger<ProjectRepository> _logger;
    private readonly ISieveProcessor _sieveProcessor;

    public ProjectRepository(SQLContext dbContext, ILogger<ProjectRepository> logger, ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
    {
        _logger = logger;
        _sieveProcessor = sieveProcessor;


    }

    public async Task<IReadOnlyList<Project>> GetProjectsAsync(SieveModel sieveModel)
    {        var query = ApplyFilterAndSort(sieveModel);

        var result = await query.ToListAsync();
        return result;
    }

    private IQueryable<Project> ApplyFilterAndSort(SieveModel sieveModel)
    {
        var query = dbContext.Projects.AsQueryable();

        query = _sieveProcessor.Apply(sieveModel, query);

        return query;
    }

}