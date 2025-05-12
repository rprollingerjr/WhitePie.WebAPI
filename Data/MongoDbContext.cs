using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WhitePie.WebAPI.Models;

namespace WhitePie.WebAPI.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoDatabase Database => _database;
    public IMongoCollection<Moment> Moments => _database.GetCollection<Moment>("Moments");
    public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
}
