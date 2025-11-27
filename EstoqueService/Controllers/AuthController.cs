using EstoqueService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(IConfiguration configuration)
        {
            // Instanciar o TokenService com as configurações do appsettings.json
            _tokenService = new TokenService(
                configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key"),
                configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer"),
                configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience")
            );
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validação simples de usuário (substituir por validação real)
            if (request.Username == "admin" && request.Password == "admin123")
            {
                var token = _tokenService.GenerateToken(request.Username, "Administrador");
                return Ok(new { Token = token });
            }
            else if (request.Username == "cliente" && request.Password == "cliente123")
            {
                var token = _tokenService.GenerateToken(request.Username, "Cliente");
                return Ok(new { Token = token });
            }

            return Unauthorized("Usuário ou senha inválidos");
        }
    }

    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}