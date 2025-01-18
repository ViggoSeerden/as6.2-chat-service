using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChatServiceBusiness.Services;

public class MessageReceiver : IHostedService
{
    private static ConcurrentDictionary<string, string> _consumedMessage = new();

    private readonly IChannel _channel;

    public MessageReceiver(IChannel channel)
    {
        _channel = channel;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Declare a topic exchange
        await _channel.ExchangeDeclareAsync(exchange: "osso-exchange", type: ExchangeType.Topic, durable: true,
            autoDelete: false, cancellationToken: cancellationToken);

        // Declare a queue and bind it to the exchange with a routing key pattern
        var queueDeclareResult = await _channel.QueueDeclareAsync(queue: "", durable: true, exclusive: false,
            autoDelete: false, cancellationToken: cancellationToken);
        var queueName = queueDeclareResult.QueueName;

        await _channel.QueueBindAsync(queue: queueName, exchange: "osso-exchange", routingKey: "payment.response.*",
            cancellationToken: cancellationToken); //request id
        await _channel.QueueBindAsync(queue: queueName, exchange: "osso-exchange", routingKey: "user.response.*",
            cancellationToken: cancellationToken); //request id
        await _channel.QueueBindAsync(queue: queueName, exchange: "osso-exchange", routingKey: "post.response.*",
            cancellationToken: cancellationToken); //request id
        await _channel.QueueBindAsync(queue: queueName, exchange: "osso-exchange", routingKey: "email.response.*",
            cancellationToken: cancellationToken); //request id
        Console.WriteLine(" [*] Now listening to messages in osso-exchange'");
        AsyncEventingBasicConsumer consumer = new(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received '{ea.RoutingKey}':'{message}'");

                var routingKeyParts =
                    ea.RoutingKey.Split('.'); // 0 is topic, 1 is request/response, 2 is requestId

                _consumedMessage[routingKeyParts[2]] = message;

                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true,
                    cancellationToken: cancellationToken);
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        };
        await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer,
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public static string? GetConsumedMessage(string requestId)
    {
        _consumedMessage.TryGetValue(requestId, out var result);
        return result; // Return null if not found
    }
}