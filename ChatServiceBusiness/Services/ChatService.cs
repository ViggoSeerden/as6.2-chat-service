using ChatServiceBusiness.Interfaces;
using ChatServiceBusiness.Models;

namespace ChatServiceBusiness.Services;

public class ChatService(IChatRepository chatRepository)
{
    private readonly IChatRepository _chatRepository = chatRepository;
    
    public Task<List<Chat>> GetAllChatsAsync() => _chatRepository.GetAllAsync();
    public Task<Chat> GetChatByIdAsync(Guid id) => _chatRepository.GetByIdAsync(id);
    public Task AddChatAsync(Chat chat) => _chatRepository.AddAsync(chat);
    public Task UpdateChatAsync(Chat chat) => _chatRepository.UpdateAsync(chat);
    public Task DeleteChatAsync(Guid id) => _chatRepository.DeleteAsync(id);
}