using System;

namespace EmailService.Services;

public interface IEmailService
{
    Task SendEmailService(string email, string username);
}
