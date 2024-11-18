using ChatServiceBusiness.Models;

namespace ChatServiceBusiness.Interfaces;

public interface IChatRepository
{
    Task<List<Chat>> GetAllAsync();
    Task<Chat> GetByIdAsync(Guid id);
    Task AddAsync(Chat chat);
    Task UpdateAsync(Chat chat);
    Task DeleteAsync(Guid id);
}