using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignSphere.Infrastructure.Services;
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