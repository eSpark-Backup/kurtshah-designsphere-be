using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;

namespace designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;

public interface IDeviceRepository : IAsyncRepository<Device>
{
    Task<Device> GetDeviceCheck(string devicecode);
}
