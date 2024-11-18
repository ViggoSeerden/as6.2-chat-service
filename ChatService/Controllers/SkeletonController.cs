using ChatServiceBusiness.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers;

[ApiController]
[Route("api/skeleton")]
public class SkeletonController : ControllerBase
{
    private MessageProducer _sender = new();
    
    [HttpGet("")]
    public string GetSkeletonMessage()
    {
        return "This is the Chat Service Skeleton endpoint.";
    }
    
    [HttpGet("payment")]
    public string? SendSkeletonPayment()
    {
        _sender.SendMessage();
        var response = MessageReceiver.GetConsumedMessage();
        return response;
    }
}