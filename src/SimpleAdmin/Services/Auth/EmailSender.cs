using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace SimpleAdmin.Services.Auth;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            if (smtpSettings == null)
            {
                _logger.LogError("SMTP settings not found in configuration");
                return;
            }

            using var client = new SmtpClient(smtpSettings.Server, smtpSettings.Port)
            {
                EnableSsl = smtpSettings.EnableSsl,
                Credentials = new System.Net.NetworkCredential(smtpSettings.Username, smtpSettings.Password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings.FromEmail, smtpSettings.FromName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation($"Email sent successfully to {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {email}");
            throw;
        }
    }
}

public class SmtpSettings
{
    public string Server { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
    public string FromName { get; set; } = null!;
    public bool EnableSsl { get; set; }
} 