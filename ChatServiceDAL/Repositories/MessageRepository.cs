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
        var filter = Builders<Message>.Filter.Eq(message => message.Id, id);
        return _messageCollection.Find(filter).FirstOrDefaultAsync();
    }

    public Task AddAsync(Message message)
    {
        return _messageCollection.InsertOneAsync(message);
    }

    public Task UpdateAsync(Guid id, Message updatedMessage)
    {
        var filter = Builders<Message>.Filter.Eq(message => message.Id, id);
        var update = Builders<Message>.Update
            .Set(message => message.Content, updatedMessage.Content);
        
        return _messageCollection.UpdateOneAsync(filter, update);
    }

    public Task DeleteAsync(Guid id)
    {
        var filter = Builders<Message>.Filter.Eq(message => message.Id, id);
        return _messageCollection.DeleteOneAsync(filter);
    }
}