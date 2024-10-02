using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;

namespace designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;

public interface IExExUserIdentityRepository
{
    Task<string> AddUser(ExUserIdentity user);
    void DeleteUser(ExUserIdentity user);
}
