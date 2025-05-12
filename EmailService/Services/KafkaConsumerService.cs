using System;
using System.Text.Json;
using Confluent.Kafka;

namespace EmailService.Services;

public class KafkaConsumerService :BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IEmailService _emailService;
    private const string Topic = "user-registered";

    public KafkaConsumerService(IEmailService emailService)
    {
        var config = new ConsumerConfig{
            BootstrapServers = "localhost:9092",
            GroupId = "email-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _emailService = emailService;
        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(Topic);
        while(!stoppingToken.IsCancellationRequested){
            try
            {
                var consumerResult = _consumer.Consume(stoppingToken);
                var message = JsonSerializer.Deserialize<UserRegistered>(consumerResult.Message.Value);
                if(message != null){
                    await _emailService.SendEmailService(message.Email, message.Username);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.Message}");
            }
        }
    }
    public override void Dispose()
    {
        _consumer?.Dispose();
        base.Dispose();
    }
}


public class UserRegistered{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public DateTime Timestamp { get; set; }
}