using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using DesignSphere.Application.Contracts.Services;
namespace DesignSphere.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IEmailConfig _emailConfig;

    public EmailService(IOptions<EmailConfig> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }


    //public string GetMessageBody(EmailGrantRequest model, string verificationUrl)
    //{
    //    var emailBody = $"Hi {model.FirstName},<br><br>Please confirm your email by clicking the link below:<br><a href='{verificationUrl}'>Verify Email</a>";

    //    return emailBody;
    //}


    //public string GetMessageBodyForOtpRequest(EmailGrantRequest model, string otp)
    //{
    //    var emailBody = $"Hi {model.FirstName},<br><br>verification otp:<br><a>{otp}</a>";

    //    return emailBody;
    //}
    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient(_emailConfig.SmtpServer)
            {
                Port = _emailConfig.SmtpPort,
                Credentials = new NetworkCredential(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
    }
}
