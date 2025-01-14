//using DesignSphere.Application.Contracts;
//using DesignSphere.Domain.Entites;
//using DesignSphere.Infrastructure.Persistence;
//using DesignSphere.Infrastructure.Repositories;
//using Microsoft.Extensions.Logging;
//using SieveCustom.Services;


//namespace DesignSphere.Infrastructure.Repositries;

//public class ContributerRepository : RepositoryBase<ProjectContributer>, IContributerRepository
//{
//    private readonly ILogger<ContributerRepository> _logger;

//    public ContributerRepository(SQLContext dbContext, ILogger<ContributerRepository> logger, ISieveProcessor sieveProcessor)
//        : base(dbContext, sieveProcessor)
//    {
//        _logger = logger;
//    }
//}
