using Microsoft.AspNetCore.Mvc;
using APIGateway.Services;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validacao de credenciais hardcoded (pode ser expandido para banco de dados)
            if (request.Username == "admin" && request.Password == "admin123")
            {
                var token = _tokenService.GenerateToken("admin", "Administrador");
                return Ok(new { Token = token });
            }
            else if (request.Username == "cliente" && request.Password == "cliente123")
            {
                var token = _tokenService.GenerateToken("cliente", "Cliente");
                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Credenciais invalidas" });
        }
    }

    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
