using Microsoft.AspNetCore.Mvc;
using WhitePie.WebAPI.Models;
using WhitePie.WebAPI.Services;

namespace WhitePie.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AboutController : ControllerBase
{
    private readonly AboutService _service;

    public AboutController(AboutService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<AboutContent>> Get()
    {
        var about = await _service.GetAsync();

        if (about == null)
        {
            // Return an empty placeholder object (no 404)
            return Ok(new AboutContent());
        }

        return Ok(about);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AboutContent content)
    {
        if (string.IsNullOrWhiteSpace(content.Title))
            return BadRequest("Title is required.");

        content.LastUpdated = DateTime.UtcNow; // 👈 Optional but helpful

        var success = await _service.UpdateAsync(content);
        return success ? Ok("Updated") : StatusCode(500, "Failed to update.");
    }
}
