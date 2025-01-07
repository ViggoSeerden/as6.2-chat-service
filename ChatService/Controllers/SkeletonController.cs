using ChatServiceBusiness.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers;

[ApiController]
[Route("api/skeleton")]
public class SkeletonController : ControllerBase
{
    private readonly MessageProducer _sender;

    public SkeletonController(MessageProducer sender)
    {
        _sender = sender;
    }

    [HttpGet("")]
    public string GetSkeletonMessage()
    {
        return "This is the Chat Service Skeleton endpoint.";
    }

    [HttpGet("payment")]
    public async Task<string?> SendSkeletonPayment(int amount)
    {
        var requestId = Guid.NewGuid();
        _sender.SendMessage($"payment.request.{requestId}.{amount}", "PaymentRequest");

        var timeout = TimeSpan.FromSeconds(5);
        var cancellationTokenSource = new CancellationTokenSource(timeout);
        var cancellationToken = cancellationTokenSource.Token;

        // Try to get the consumed message within the timeout period
        while (!cancellationToken.IsCancellationRequested)
        {
            var response = MessageReceiver.GetConsumedMessage(requestId.ToString());
            if (response != null)
            {
                return response;
            }

            await Task.Delay(100, cancellationToken);
        }

        return "The response took too long to arrive.";
    }
}