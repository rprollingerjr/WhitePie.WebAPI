using Microsoft.AspNetCore.Mvc;
using WhitePie.WebAPI.DTOs;
using WhitePie.WebAPI.Services;

namespace WhitePie.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly EmailService _emailService;

    public ContactController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Send(ContactRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Name) ||
            string.IsNullOrWhiteSpace(dto.Message))
        {
            return BadRequest("All fields are required.");
        }

        await _emailService.SendContactEmailAsync(dto);
        return Ok("Message sent!");
    }
}
