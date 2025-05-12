using MongoDB.Driver;
using WhitePie.WebAPI.Data;
using WhitePie.WebAPI.Models;

namespace WhitePie.WebAPI.Services;

public class EventService
{
    private readonly IMongoCollection<Event> _collection;

    public EventService(MongoDbContext context)
    {
        _collection = context.Database.GetCollection<Event>("Events");
    }

    public async Task<List<Event>> GetAllAsync()
    {
        var now = DateTime.UtcNow;
        var filter = Builders<Event>.Filter.Gte(e => e.StartTime, now);
        var sort = Builders<Event>.Sort.Ascending(e => e.StartTime);
        return await _collection.Find(filter).Sort(sort).ToListAsync();
    }

    public async Task<Event?> GetAsync(string id)
    {
        return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Event newEvent)
    {
        await _collection.InsertOneAsync(newEvent);
    }

    public async Task<bool> UpdateAsync(string id, Event updated)
    {
        var result = await _collection.ReplaceOneAsync(e => e.Id == id, updated);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(e => e.Id == id);
        return result.DeletedCount > 0;
    }
}
