
using System.Text.Json;
using Confluent.Kafka;

namespace AuthenticationService.Services;

public class KafkaProducerService{
    private readonly IProducer<string, string> _producer;
    private const string Topic = "user-registered";
    public KafkaProducerService()
    {
        var config = new ProducerConfig{
            BootstrapServers = "localhost:9092"
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    //viết hàm để send event tới kafka message broker
    public async Task PublishUserRegisterdEvent(string email, string username){
        //defind new message 
        var message = new {
            Email= email,
            Username= username,
            Timestamp = DateTime.UtcNow
        };
        //convert về dạng json
        var jsonMessage = JsonSerializer.Serialize(message);
        //gửi message tới kafka xử lý
        await _producer.ProduceAsync(Topic, new Message<string, string>{
            Key = email,
            Value = jsonMessage
        });
    }
    // viết hàm để close producer, clear bộ nhớ
    public void Dispose(){
        _producer.Dispose();
    }
}