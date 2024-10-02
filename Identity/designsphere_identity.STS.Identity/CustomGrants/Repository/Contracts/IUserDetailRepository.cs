using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;

namespace designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
public interface IUserDetailRepository : IAsyncRepository<UserDetail>
{
    Task<UserDetail> GetDetailOfUser(string username);
    Task UpdateAndLog(UserDetail userDetail);
}
