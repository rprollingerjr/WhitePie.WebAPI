using MongoDB.Driver;
using WhitePie.WebAPI.Data;
using WhitePie.WebAPI.DTOs;
using WhitePie.WebAPI.Models;

public class MomentService
{
    private readonly MongoDbContext _context;

    public MomentService(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Moment>> GetAllAsync()
    {
        var sort = Builders<Moment>.Sort.Ascending(m => m.DisplayOrder);
        return await _context.Moments.Find(_ => true).Sort(sort).ToListAsync();
    }

    public async Task<(List<Moment> Moments, long TotalCount)> GetPaginatedAsync(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
            throw new ArgumentException("Page and pageSize must be greater than zero.");

        var skip = (page - 1) * pageSize;

        var totalCount = await _context.Moments.CountDocumentsAsync(_ => true);

        var sort = Builders<Moment>.Sort.Ascending(m => m.DisplayOrder);

        var moments = await _context.Moments
            .Find(_ => true)
            .Sort(sort)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();

        return (moments, totalCount);
    }

    public async Task<Moment> CreateAsync(MomentCreateDto dto)
    {
        var moment = new Moment
        {
            Title = dto.Title,
            Description = dto.Description,
            ImageId = dto.ImageId
        };
        moment.DisplayOrder = (int)await _context.Moments.CountDocumentsAsync(_ => true);
        await _context.Moments.InsertOneAsync(moment);
        return moment;
    }

    public async Task<Moment?> UpdateAsync(string id, MomentUpdateDto dto)
    {
        var filter = Builders<Moment>.Filter.Eq(m => m.Id, id);
        var update = Builders<Moment>.Update
            .Set(m => m.Title, dto.Title)
            .Set(m => m.Description, dto.Description ?? "");

        var result = await _context.Moments.UpdateOneAsync(filter, update);
        if (result.MatchedCount == 0)
            return null;

        return await _context.Moments.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _context.Moments.DeleteOneAsync(m => m.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> ReorderAsync(List<string> orderedIds)
    {
        var bulkOps = new List<WriteModel<Moment>>();

        for (int i = 0; i < orderedIds.Count; i++)
        {
            var filter = Builders<Moment>.Filter.Eq(m => m.Id, orderedIds[i]);
            var update = Builders<Moment>.Update.Set(m => m.DisplayOrder, i);

            bulkOps.Add(new UpdateOneModel<Moment>(filter, update));
        }

        if (bulkOps.Count == 0) return false;

        var result = await _context.Moments.BulkWriteAsync(bulkOps);
        return result.IsAcknowledged;
    }
}
