using MongoDB.Driver;
using WhitePie.WebAPI.Data;
using WhitePie.WebAPI.Models;

namespace WhitePie.WebAPI.Services;

public class BookingLogService
{
    private readonly IMongoCollection<BookingLog> _collection;

    public BookingLogService(MongoDbContext dbContext)
    {
        _collection = dbContext.Database.GetCollection<BookingLog>("BookingLogs");
    }

    public async Task SaveAsync(BookingLog log)
    {
        await _collection.InsertOneAsync(log);
    }
}
