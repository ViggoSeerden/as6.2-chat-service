using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers;

[ApiController]
[Route("chats")]
public class SkeletonController : ControllerBase
{
    [HttpGet("skeleton")]
    public string GetSkeletonMessage()
    {
        return "This is the Chat Service Skeleton endpoint.";
    }
}