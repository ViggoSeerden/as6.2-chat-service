using ChatServiceBusiness.Interfaces;
using ChatServiceBusiness.Models;

namespace ChatServiceBusiness.Services;

public class MessageService(IMessageRepository messageRepository)
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    
    public Task<List<Message>> GetAllMessagesAsync() => _messageRepository.GetAllAsync();
    public Task<Message> GetMessageByIdAsync(Guid id) => _messageRepository.GetByIdAsync(id);
    public Task AddMessageAsync(Message msg) => _messageRepository.AddAsync(msg);
    public Task UpdateMessageAsync(Guid id, Message msg) => _messageRepository.UpdateAsync(id, msg);
    public Task DeleteMessageAsync(Guid id) => _messageRepository.DeleteAsync(id);
}