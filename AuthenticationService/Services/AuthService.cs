using System;
using AuthenticationService.Data;
using AuthenticationService.DTOs;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Services;

public class AuthService
{
    private readonly AuthDbContext _context;
    //private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly KafkaProducerService _kafkaProducerService;
    public AuthService(AuthDbContext context,
     //IHttpClientFactory httpClientFactory,
     KafkaProducerService kafkaProducerService,
     IConfiguration configuration)
    {
        _context = context;
        _kafkaProducerService = kafkaProducerService;
        _configuration = configuration;
        // _httpClientFactory = httpClientFactory;
    }
    public async Task<AuthResponse> Register(RegisterDTO registerDTO){
        if(await _context.Users.AnyAsync(u => u.Email == registerDTO.Email)){
            throw new Exception("Email already exists");
        }
        var user = new User{
            UserName = registerDTO.UserName,
            Email = registerDTO.Email,
            PasswordHash = registerDTO.Password
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // var client = _httpClientFactory.CreateClient("EmailService");
        // var result = await client.PostAsJsonAsync("/api/Email/send-email-welcome", new {
        //     Email= user.Email,
        //     Username = user.UserName
        // });
        // var content = await result.Content.ReadAsStringAsync();
        //dùng kafka producer 
        await _kafkaProducerService.PublishUserRegisterdEvent(user.Email, user.UserName);
        
        return new AuthResponse{
            Token = "test",
            UserName = user.UserName,
            Email = user.Email
        };
    }
}
