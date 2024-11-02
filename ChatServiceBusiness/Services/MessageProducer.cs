using System.Text;
using RabbitMQ.Client;

namespace ChatServiceBusiness.Services;

public class MessageProducer
{
    private readonly IModel _channel;

    public MessageProducer()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.QueueDeclare(queue: "payment-skeleton",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    
    public void SendMessage()
    {
        const string message = "Send a Payment";
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: string.Empty,
            routingKey: "payment-skeleton",
            basicProperties: null,
            body: body);
        Console.WriteLine($" [x] Sent {message}");
    }
}