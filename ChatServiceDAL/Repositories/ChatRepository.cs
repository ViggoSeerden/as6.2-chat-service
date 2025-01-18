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
        var filter = Builders<Chat>.Filter.Eq(chat => chat.Id, id);
        return _chatCollection.Find(filter).FirstOrDefaultAsync();
    }

    public Task AddAsync(Chat chat)
    {
        return _chatCollection.InsertOneAsync(chat);
    }

    public Task UpdateAsync(Guid id, Chat updatedChat)
    {
        var filter = Builders<Chat>.Filter.Eq(chat => chat.Id, id);
        var update = Builders<Chat>.Update
            .Set(chat => chat.Status, updatedChat.Status);
        
        return _chatCollection.UpdateOneAsync(filter, update);
    }
    public Task DeleteAsync(Guid id)
    {
        var filter = Builders<Chat>.Filter.Eq(chat => chat.Id, id);
        return _chatCollection.DeleteOneAsync(filter);
    }
}