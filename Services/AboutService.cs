using MongoDB.Driver;
using WhitePie.WebAPI.Data;
using WhitePie.WebAPI.Models;

namespace WhitePie.WebAPI.Services;

public class AboutService
{
    private readonly IMongoCollection<AboutContent> _collection;

    public AboutService(MongoDbContext context)
    {
        _collection = context.Database.GetCollection<AboutContent>("AboutContent");
    }

    public async Task<AboutContent?> GetAsync()
    {
        return await _collection.Find(a => a.Id == "about").FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(AboutContent updated)
    {
        updated.Id = "about"; // 🔐 Always enforce the singleton ID
        updated.LastUpdated = DateTime.UtcNow;

        var result = await _collection.ReplaceOneAsync(
            a => a.Id == "about",
            updated,
            new ReplaceOptions { IsUpsert = true }
        );

        return result.IsAcknowledged;
    }
}
