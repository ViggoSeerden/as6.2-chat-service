using ChatServiceBusiness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers;

[ApiController]
[Route("api/chats")]
public class ChatController(ChatServiceBusiness.Services.ChatService chatService) : ControllerBase
{
    [HttpGet("")]
    [Authorize("read:chats")]
    public async Task<IActionResult> GetAllChats()
    {
        var chats = await chatService.GetAllChatsAsync();
        return Ok(chats);
    }
    
    [HttpGet("{id}")]
    [Authorize("read:chat")]
    public async Task<IActionResult> GetChatById(Guid id)
    {
        var chat = await chatService.GetChatByIdAsync(id);
        return Ok(chat);
    }
        
    [HttpPost("")]
    [Authorize("write:add_chat")]
    public async Task<IActionResult> AddChat([FromBody] Chat chat)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await chatService.AddChatAsync(chat);
        return NoContent();
    }
        
    [HttpPut("{id}")]
    [Authorize("write:update_chat")]
    public async Task<IActionResult> UpdateChat(Guid id, [FromBody] Chat updatedChat)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var chat = await chatService.GetChatByIdAsync(id);
        if (chat == null)
            return NotFound();
            
        await chatService.UpdateChatAsync(id, updatedChat);
        return NoContent();
    }
        
    [HttpDelete("{id}")]
    [Authorize("write:delete_chat")]
    public async Task<IActionResult> DeleteChat(Guid id)
    {
        await chatService.DeleteChatAsync(id);
        return Ok();
    }
}