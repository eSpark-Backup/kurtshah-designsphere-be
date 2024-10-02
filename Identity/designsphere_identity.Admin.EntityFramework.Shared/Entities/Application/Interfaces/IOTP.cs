using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Enums;

namespace designsphere_identity.Admin.EntityFramework.Shared.Entities.Application.Interfaces;

public interface IOTPTokenResponse
{
    string ProtectToken { get; set; }
}

public interface IOTPToken : IOTPTokenResponse
{
    string Otp { get; set; }
}

public interface IName
{
    string Name { get; set; }
}

public interface IGoogleRequest
{
    string IdToken { get; set; }
}

public interface IOTP
{
    byte OTPRetries { get; set; }
    EOTPStatus OTPStatus { get; set; }
}
