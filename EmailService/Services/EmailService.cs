using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EmailService.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }
    public async Task SendEmailService(string email, string username){
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Your App", _config["Email:From"]));
        message.To.Add(new MailboxAddress(username, email));
        message.Subject = "Welcome to app";

        var bodyBuilder = new BodyBuilder{
            HtmlBody = $@"
                <h1>Welcome to Our App!</h1>
                <p>Dear {username},</p>
                <p>Thank you for registering with us. We're excited to have you on board!</p>
                <p>Best regards,<br>Your App Team</p>"
        };
        message.Body = bodyBuilder.ToMessageBody();
        using var client = new SmtpClient();

        await client.ConnectAsync(
            _config["Email:SmtpServer"],
            int.Parse(_config["Email:Port"]!),
            SecureSocketOptions.StartTls
        );
        await client.AuthenticateAsync(
            _config["Email:Username"],
            _config["Email:Password"]
        );
        await client.SendAsync(message);
        await client.DisconnectAsync(
            true
        );
    }
    public string SendWelcomeEmail(string username, string email){
        var test = $"username :{username}, email: {email}";
        return test;
    }
}
