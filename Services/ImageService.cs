using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using WhitePie.WebAPI.Data;

namespace WhitePie.WebAPI.Services;

public class ImageService
{
    private readonly GridFSBucket _bucket;

    public ImageService(MongoDbContext dbContext)
    {
        _bucket = new GridFSBucket(dbContext.Database);
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        if (file.Length == 0)
            throw new ArgumentException("Empty file upload");

        try
        {
            using var stream = file.OpenReadStream();
            var id = await _bucket.UploadFromStreamAsync(file.FileName, stream);
            Console.WriteLine($"[GridFS] Image uploaded: {id}");
            return id.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GridFS upload failed: {ex.Message}");
            throw;
        }
    }

    public async Task<Stream?> GetImageByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId)) return null;
        return await _bucket.OpenDownloadStreamAsync(objectId);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId)) return false;

        await _bucket.DeleteAsync(objectId);
        return true;
    }
}
