using Microsoft.AspNetCore.Mvc;
using WhitePie.WebAPI.Models;
using WhitePie.WebAPI.Services;

namespace WhitePie.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly ResendEmailService _email;
    private readonly BookingLogService _log;

    public BookingController(ResendEmailService email,
                                BookingLogService log)
    {
        _email = email;
        _log = log;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BookingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Missing email or message");

        var managementBody = $@"
            <h3>New Booking Request</h3>
            <p><strong>Name:</strong> {request.Name}</p>
            <p><strong>Email:</strong> {request.Email}</p>
            <p><strong>Phone:</strong> {request.Phone}</p>
            <p><strong>Message:</strong><br/>{request.Message}</p>
        ";

        var userBody = $@"
            <div style='font-family:Arial,sans-serif;max-width:600px;margin:0 auto;padding:20px;'>
                <img src='https://ediblemami.com/assets/logo.png' alt='EdibleMami Logo' style='max-width:150px;' />
                <h2>Thanks for contacting EdibleMami 🍕</h2>
                <p>Hey {request.Name},</p>
                <p>Thanks for reaching out! We received your booking request and will get back to you shortly.</p>
                <h4>Your Request:</h4>
                <ul>
                    <li><strong>Email:</strong> {request.Email}</li>
                    <li><strong>Phone:</strong> {request.Phone}</li>
                    <li><strong>Message:</strong><br/>{request.Message}</li>
                </ul>
                <p>In the meantime, follow us on 
                <a href='https://www.instagram.com/ediblemamipizza/'>Instagram</a> or visit 
                <a href='https://ediblemami.com'>ediblemami.com</a></p>
                <p>Cheers,<br/>🍕 The EdibleMami Team</p>
            </div>
        ";

        // Send to management
        var adminOk = await _email.SendEmailAsync("📬 New Booking Request", managementBody);

        // Send to user
        var userOk = await _email.SendEmailAsync("✅ Thanks for contacting EdibleMami!", userBody);

        await _log.SaveAsync(new BookingLog
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Message = request.Message
        });

        return (adminOk && userOk) ? Ok("Emails sent!") : StatusCode(500, "Email failed");
    }
}
