//using DesignSphere.Application.Contracts.Services;
//using DesignSphere.Domain.Entites;
//using DesignSphere.Infrastructure.Persistence;
//using DesignSphere.Infrastructure.Repositories;
//using Microsoft.Extensions.Logging;
//using SieveCustom.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DesignSphere.Infrastructure.Repositries.Services;


//public class ActivityRepository  : RepositoryBase<Conversation>, IActivityRepository
//{

//    private readonly ILogger<DoorOpeningRepository> _logger;

//    public ActivityRepository(SQLContext dbContext, ILogger<DoorOpeningRepository> logger, ISieveProcessor sieveProcessor)
//        : base(dbContext, sieveProcessor)
//    {
//        _logger = logger;
//    }
//    //public async Task<List<ActivityFeed>> GetByProjectIdAsync(int projectId)
//    //{
//    //    return await _context.ActivityFeeds
//    //        .Where(a => a.ProjectId == projectId)
//    //        .OrderByDescending(a => a.Timestamp)
//    //        .ToListAsync();
//    //}
//}
