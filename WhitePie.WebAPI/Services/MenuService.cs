using MongoDB.Driver;
using WhitePie.WebAPI.Data;
using WhitePie.WebAPI.Models;

namespace WhitePie.WebAPI.Services;

public class MenuService
{
    private readonly IMongoCollection<MenuConfig> _collection;

    public MenuService(MongoDbContext dbContext)
    {
        _collection = dbContext.Database.GetCollection<MenuConfig>("MenuConfig");
    }

    public async Task<MenuConfig?> GetAsync()
    {
        return await _collection.Find(m => m.Id == "menu-config").FirstOrDefaultAsync();
    }

    public async Task<bool> SaveAsync(MenuConfig config)
    {
        var result = await _collection.ReplaceOneAsync(
            m => m.Id == "menu-config",
            config,
            new ReplaceOptions { IsUpsert = true }
        );

        return result.IsAcknowledged;
    }
}
