namespace SimpleAdmin.Services.Auth;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
} 