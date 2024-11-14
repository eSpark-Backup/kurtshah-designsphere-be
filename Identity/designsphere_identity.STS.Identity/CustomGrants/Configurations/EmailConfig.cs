namespace designsphere_identity.STS.Identity.CustomGrants.Configurations;

public interface IEmailConfig
{
    string SmtpServer { get; set; }
    int SmtpPort { get; set; }
    string SmtpUsername { get; set; }
    string SmtpPassword { get; set; }
    string SenderEmail { get; set; }
}

public class EmailConfig : IEmailConfig
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public string SenderEmail { get; set; }
}
