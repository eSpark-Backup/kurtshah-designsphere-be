namespace designsphere_identity.STS.Identity.CustomGrants.Configurations;

public interface ISMSConfig
{
    string Secret { get; set; }
    string ServerAddress { get; set; }
}

public class SMSConfig : ISMSConfig
{
    public string Secret { get; set; }
    public string ServerAddress { get; set; }
}
