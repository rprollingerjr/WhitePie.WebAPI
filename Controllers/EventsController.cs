using Microsoft.AspNetCore.Mvc;
using WhitePie.WebAPI.Models;
using WhitePie.WebAPI.Services;
using WhitePie.WebAPI.DTOs;

namespace WhitePie.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly EventService _service;

    public EventsController(EventService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Event>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Event newEvent)
    {
        if (string.IsNullOrWhiteSpace(newEvent.EventTitle))
            return BadRequest("Event title is required.");

        await _service.CreateAsync(newEvent);
        return Ok(newEvent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Event updated)
    {
        var existing = await _service.GetAsync(id);
        if (existing is null)
            return NotFound();

        updated.Id = id;
        await _service.UpdateAsync(id, updated);
        return Ok("Updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
