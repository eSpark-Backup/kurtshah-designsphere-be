using Skoruba.IdentityServer4.Shared.Configuration.Configuration.Identity;
using designsphere_identity.STS.Identity.Configuration.Interfaces;

namespace designsphere_identity.STS.Identity.Configuration;

public class RootConfiguration : IRootConfiguration
{
    public AdminConfiguration AdminConfiguration { get; } = new AdminConfiguration();
    public RegisterConfiguration RegisterConfiguration { get; } = new RegisterConfiguration();
}
