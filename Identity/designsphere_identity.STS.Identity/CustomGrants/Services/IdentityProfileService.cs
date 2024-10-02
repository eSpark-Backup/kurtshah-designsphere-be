using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Identity;

namespace designsphere_identity.STS.Identity.CustomGrants.Services;

public class IdentityProfileService<TUser> : IProfileService
      where TUser : class
{
    private readonly IUserClaimsPrincipalFactory<UserIdentity> _userClaimsPrincipalFactory;
    private readonly UserManager<UserIdentity> _userMgr;
    private readonly RoleManager<UserIdentityRole> _roleMgr;

    public IdentityProfileService(
        UserManager<UserIdentity> userMgr,
        RoleManager<UserIdentityRole> roleMgr,
        IUserClaimsPrincipalFactory<UserIdentity> userClaimsPrincipalFactory)
    {
        _userMgr = userMgr;
        _roleMgr = roleMgr;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string sub = context.Subject.GetSubjectId();
        UserIdentity user = await _userMgr.FindByIdAsync(sub);
        ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

        List<Claim> claims = userClaims.Claims.ToList();
        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

        if (_userMgr.SupportsUserRole)
        {
            IList<string> roles = await _userMgr.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                if (_roleMgr.SupportsRoleClaims)
                {
                    UserIdentityRole role = await _roleMgr.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        claims.AddRange(await _roleMgr.GetClaimsAsync(role));
                    }
                }
            }
        }

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string sub = context.Subject.GetSubjectId();
        UserIdentity user = await _userMgr.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}
