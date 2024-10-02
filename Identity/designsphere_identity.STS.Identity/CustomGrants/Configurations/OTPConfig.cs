namespace designsphere_identity.STS.Identity.CustomGrants.Configurations;

public interface IOTPConfig
{
    string Secret { get; set; }
    int Validity { get; set; }
    int Retries { get; set; }
}

public class OTPConfig : IOTPConfig
{
    public string Secret { get; set; }
    public int Validity { get; set; }
    public int Retries { get; set; }
}
