using Microsoft.AspNetCore.Mvc;
using WhitePie.WebAPI.DTOs;
using WhitePie.WebAPI.Models;
using WhitePie.WebAPI.Services;

namespace WhitePie.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MomentsController : ControllerBase
{
    private readonly MomentService _service;

    public MomentsController(MomentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var (moments, totalCount) = await _service.GetPaginatedAsync(page, pageSize);

            return Ok(new
            {
                moments,
                totalCount
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<Moment>> CreateMomentWithImage(
        [FromForm] string title,
        [FromForm] string? description,
        [FromForm] IFormFile file,
        [FromServices] ImageService imageService)
    {
        if (file == null || file.Length == 0 || string.IsNullOrWhiteSpace(title))
            return BadRequest("Image and title are required.");

        var imageId = await imageService.UploadAsync(file);

        var moment = new Moment
        {
            Title = title,
            Description = description,
            ImageId = imageId
        };

        await _service.CreateAsync(new DTOs.MomentCreateDto
        {
            Title = title,
            Description = description,
            ImageId = imageId
        });

        return CreatedAtAction(nameof(Get), new { id = moment.Id }, moment);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<Moment>> Update(string id, [FromBody] MomentUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPost("reorder")]
    public async Task<IActionResult> Reorder([FromBody] List<string> orderedIds)
    {
        if (orderedIds == null || orderedIds.Count == 0)
            return BadRequest("Must provide at least one ID.");

        var success = await _service.ReorderAsync(orderedIds);
        return success ? Ok() : StatusCode(500, "Could not reorder moments.");
    }
}
