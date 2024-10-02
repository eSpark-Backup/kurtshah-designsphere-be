using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BuildingBlocks.Common.Filters;
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(JwtClaimTypes.PhoneNumber, "03333037123"),
            new Claim(JwtClaimTypes.ClientId, "tst_client"),
            new Claim("role", "customer1"),
            new Claim(JwtClaimTypes.Subject, "8f460576-4907-4798-8488-4efea8b88289")
        };
        var identity = new ClaimsIdentity(claims, "Testauthtype");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Testscheme");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
