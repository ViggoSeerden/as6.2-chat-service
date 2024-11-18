using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers;

[ApiController]
[Route("api/chats")]
public class ChatController(ChatServiceBusiness.Services.ChatService chatService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllChats()
    {
        var chats = await chatService.GetAllChatsAsync();
        return Ok(chats);
    }
}