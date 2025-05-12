using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WhitePie.WebAPI.Models;

namespace WhitePie.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AdminAuthSettings _settings;

        public AuthController(IOptions<AdminAuthSettings> settings)
        {
            _settings = settings.Value;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthRequest request)
        {
            if (request.Password == _settings.Password)
                return Ok(new { success = true });

            return Unauthorized(new { success = false, message = "Invalid password" });
        }
    }

    public class AuthRequest
    {
        public string Password { get; set; }
    }
}
