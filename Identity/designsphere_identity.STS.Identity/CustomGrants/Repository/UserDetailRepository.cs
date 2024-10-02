using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Persistance;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;

namespace designsphere_identity.STS.Identity.CustomGrants.Repository;

public class UserDetailRepository : RepositoryBase<UserDetail>, IUserDetailRepository
{
    private readonly ILogger<UserDetailRepository> logger;

    public UserDetailRepository(MylocalContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserDetail> GetDetailOfUser(string username)
    {
        return await dbContext.UserDetails.OrderByDescending(_ => _.CreationTime).FirstOrDefaultAsync(_ => _.UserName == username);
    }
    public async Task UpdateAndLog(UserDetail userDetail)
    {
        if (dbContext.UserDetails.AsNoTracking().Any(_ => _.Id == userDetail.Id))
            await UpdateAsync(userDetail);
        else
            await AddAsync(userDetail);

        await dbContext.UserDetailLogs.AddAsync(new UserDetailLog()
        {
            AppHash = userDetail.AppHash,
            CreationTime = userDetail.CreationTime,
            CreatorId = userDetail.CreatorId,
            DeviceId = userDetail.DeviceId,
            UserDetailId = userDetail.Id,
            LastmodificationTime = userDetail.LastmodificationTime,
            Name = userDetail.Name,
            NotificationId = userDetail.NotificationId,
            OTPRetries = userDetail.OTPRetries,
            OTPStatus = userDetail.OTPStatus,
            UserIdentityId = userDetail.UserIdentityId,
            UserName = userDetail.UserName,
        });
        await dbContext.SaveChangesAsync();
    }
}

