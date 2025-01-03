using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChatServiceBusiness.Services;

public class MessageReceiver : IHostedService
{
    private static ConcurrentDictionary<string, string> _consumedMessage = new();
    
    private readonly IModel _channel;

    public MessageReceiver()
    {
        var factory = new ConnectionFactory() { HostName = Environment.GetEnvironmentVariable("RabbitMQ") ?? "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: "payment-skeleton",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);

            _consumedMessage["payment"] = message;
        };

        _channel.BasicConsume(queue: "payment-skeleton",
            autoAck: true, // Set to false if you want manual acknowledgments
            consumer: consumer);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    public static string? GetConsumedMessage()
    {
        _consumedMessage.TryGetValue("payment", out string? result);
        return result; // Return null if not found
    }
}