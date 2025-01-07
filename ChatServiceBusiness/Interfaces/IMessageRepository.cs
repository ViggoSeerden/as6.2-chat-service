using ChatServiceBusiness.Models;

namespace ChatServiceBusiness.Interfaces;

public interface IMessageRepository
{
    Task<List<Message>> GetAllAsync();
    Task<Message> GetByIdAsync(Guid id);
    Task AddAsync(Message msg);
    Task UpdateAsync(Guid id, Message msg);
    Task DeleteAsync(Guid id);
}