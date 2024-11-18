using MongoDB.Driver;
using ChatServiceBusiness.Interfaces;
using ChatServiceBusiness.Models;

namespace ChatServiceDAL.Repositories;

public class ChatRepository() : IChatRepository
{
    private readonly IMongoCollection<Chat> _chatCollection;

    public ChatRepository(IMongoClient mongoClient) : this()
    {
        var database = mongoClient.GetDatabase("ossodb");
        _chatCollection = database.GetCollection<Chat>("chats");
    }

    public Task<List<Chat>> GetAllAsync()
    {
        return _chatCollection.Find(_ => true).ToListAsync();
    }
    
    public Task<Chat> GetByIdAsync(Guid id)
    {
        return null;
    }

    public Task AddAsync(Chat chat)
    {
        return null;
    }

    public Task UpdateAsync(Chat chat)
    {
        return null;
    }

    public Task DeleteAsync(Guid id)
    {
        return null;
    }
}