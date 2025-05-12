using System;

namespace EmailService.DTOs;

public class EmailRequest
{
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}
