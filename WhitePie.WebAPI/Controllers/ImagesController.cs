using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.GridFS;
using WhitePie.WebAPI.Data;

[ApiController]
[Route("api/images")]
public class ImagesController : ControllerBase
{
    private readonly IGridFSBucket _gridFs;

    public ImagesController(MongoDbContext context)
    {
        _gridFs = new GridFSBucket(context.Database);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        using var stream = file.OpenReadStream();
        var id = await _gridFs.UploadFromStreamAsync(file.FileName, stream);
        return Ok(new { id = id.ToString() });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var objectId = new MongoDB.Bson.ObjectId(id);
            var stream = await _gridFs.OpenDownloadStreamAsync(objectId);
            return File(stream, "application/octet-stream");
        }
        catch
        {
            return NotFound("Image not found.");
        }
    }
}
