using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService : IEmailSender
{
    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
   
    private readonly IConfiguration configuration;

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var smtpServer = configuration["SmtpSettings:Host"];
        var smtpUsername = configuration["SmtpSettings:Username"];
        var smtpPassword = configuration["SmtpSettings:Password"];
        var smtpPort =int.Parse( configuration["SmtpSettings:Port"]);

           var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUsername,"CyberShpere Acadmy"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
