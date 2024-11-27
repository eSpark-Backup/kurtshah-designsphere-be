using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using DesignSphere.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SieveCustom.Services;

namespace DesignSphere.Infrastructure.Repositories;


public class DoorOpeningRepository : RepositoryBase<DoorOpening>, IDoorOpeningRepository
{
    private readonly ILogger<DoorOpeningRepository> _logger;

    public DoorOpeningRepository(SQLContext dbContext, ILogger<DoorOpeningRepository> logger, ISieveProcessor sieveProcessor)
        : base(dbContext, sieveProcessor)
    {
        _logger = logger;
    }
}
