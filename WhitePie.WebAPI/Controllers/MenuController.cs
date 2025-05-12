using Microsoft.AspNetCore.Mvc;
using WhitePie.WebAPI.Models;
using WhitePie.WebAPI.Services;

namespace WhitePie.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly MenuService _service;

    public MenuController(MenuService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<MenuConfig>> Get()
    {
        var menu = await _service.GetAsync();
        return menu is null ? Ok(new MenuConfig()) : Ok(menu);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] MenuConfig config)
    {
        var success = await _service.SaveAsync(config);
        return success ? Ok("Menu saved.") : StatusCode(500, "Failed to save.");
    }
}
