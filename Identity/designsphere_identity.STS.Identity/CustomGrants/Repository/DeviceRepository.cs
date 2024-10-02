using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Persistance;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;

namespace designsphere_identity.STS.Identity.CustomGrants.Repository;

public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
{
    private readonly ILogger<DeviceRepository> logger;

    public DeviceRepository(MylocalContext dbContext) : base(dbContext)
    {
    }

    public async Task<Device> GetDeviceCheck(string devicecode)
    {
        return await dbContext.Devices.FirstOrDefaultAsync(_ => _.DeviceCode == devicecode);
    }
}
