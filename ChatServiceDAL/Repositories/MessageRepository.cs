using MongoDB.Driver;
using ChatServiceBusiness.Interfaces;
using ChatServiceBusiness.Models;

namespace ChatServiceDAL.Repositories;

public class MessageRepository() : IMessageRepository
{
    private readonly IMongoCollection<Message> _messageCollection;

    public MessageRepository(IMongoClient mongoClient) : this()
    {
        var database = mongoClient.GetDatabase("ossodb");
        _messageCollection = database.GetCollection<Message>("messages");
    }

    public Task<List<Message>> GetAllAsync()
    {
        return _messageCollection.Find(_ => true).ToListAsync();
    }
    
    public Task<Message> GetByIdAsync(Guid id)
    {
        return null;
    }

    public Task AddAsync(Message msg)
    {
        return null;
    }

    public Task UpdateAsync(Message msg)
    {
        return null;
    }

    public Task DeleteAsync(Guid id)
    {
        return null;
    }
}