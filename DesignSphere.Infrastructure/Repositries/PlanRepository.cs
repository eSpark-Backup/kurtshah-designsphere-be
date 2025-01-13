using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using DesignSphere.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SieveCustom.Models;
using SieveCustom.Services;


namespace DesignSphere.Infrastructure.Repositries;

public class PlanRepository : RepositoryBase<ProjectPlan>, IPlanRepository
{
    private readonly ILogger<PlanRepository> _logger;
    private readonly ISieveProcessor _sieveProcessor;

    public PlanRepository(SQLContext dbContext, ILogger<PlanRepository> logger, ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
    {
        _logger = logger;
        _sieveProcessor = sieveProcessor;


    }

    public async Task<IReadOnlyList<ProjectPlan>> GetProjectPlansAsync(SieveModel sieveModel)
    {
        var query = ApplyFilterAndSort(sieveModel);

        var result = await query.ToListAsync();
        return result;
    }

    private IQueryable<ProjectPlan> ApplyFilterAndSort(SieveModel sieveModel)
    {
        var query = dbContext.ProjectPlans.AsQueryable();


        query = _sieveProcessor.Apply(sieveModel, query);

        return query;
    }


}