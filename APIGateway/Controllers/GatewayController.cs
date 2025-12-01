using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("api/gateway")]
    public class GatewayController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GatewayController> _logger;

        public GatewayController(HttpClient httpClient, IConfiguration configuration, ILogger<GatewayController> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        // Roteamento GET para Estoque
        [HttpGet("estoque/{*path}")]
        [Authorize]
        public async Task<IActionResult> EstoqueGet(string path)
        {
            var estoqueUrl = _configuration["Services:EstoqueService"];
            return await ForwardRequest($"{estoqueUrl}/{path}", HttpMethod.Get);
        }

        // Roteamento POST para Estoque (apenas Administrador)
        [HttpPost("estoque/{*path}")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> EstoquePost(string path, [FromBody] object body)
        {
            var estoqueUrl = _configuration["Services:EstoqueService"];
            return await ForwardRequest($"{estoqueUrl}/{path}", HttpMethod.Post, body);
        }

        // Roteamento PUT para Estoque (apenas Administrador)
        [HttpPut("estoque/{*path}")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> EstoquePut(string path, [FromBody] object body)
        {
            var estoqueUrl = _configuration["Services:EstoqueService"];
            return await ForwardRequest($"{estoqueUrl}/{path}", HttpMethod.Put, body);
        }

        // Roteamento DELETE para Estoque (apenas Administrador)
        [HttpDelete("estoque/{*path}")]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> EstoqueDelete(string path)
        {
            var estoqueUrl = _configuration["Services:EstoqueService"];
            return await ForwardRequest($"{estoqueUrl}/{path}", HttpMethod.Delete);
        }

        // Roteamento GET para Vendas
        [HttpGet("vendas/{*path}")]
        [Authorize(Policy = "Cliente")]
        public async Task<IActionResult> VendasGet(string path)
        {
            var vendasUrl = _configuration["Services:VendasService"];
            return await ForwardRequest($"{vendasUrl}/{path}", HttpMethod.Get);
        }

        // Roteamento POST para Vendas (apenas Cliente)
        [HttpPost("vendas/{*path}")]
        [Authorize(Policy = "Cliente")]
        public async Task<IActionResult> VendasPost(string path, [FromBody] object body)
        {
            var vendasUrl = _configuration["Services:VendasService"];
            return await ForwardRequest($"{vendasUrl}/{path}", HttpMethod.Post, body);
        }

        // Roteamento PUT para Vendas (apenas Cliente)
        [HttpPut("vendas/{*path}")]
        [Authorize(Policy = "Cliente")]
        public async Task<IActionResult> VendasPut(string path, [FromBody] object body)
        {
            var vendasUrl = _configuration["Services:VendasService"];
            return await ForwardRequest($"{vendasUrl}/{path}", HttpMethod.Put, body);
        }

        // Roteamento DELETE para Vendas (apenas Cliente)
        [HttpDelete("vendas/{*path}")]
        [Authorize(Policy = "Cliente")]
        public async Task<IActionResult> VendasDelete(string path)
        {
            var vendasUrl = _configuration["Services:VendasService"];
            return await ForwardRequest($"{vendasUrl}/{path}", HttpMethod.Delete);
        }

        private async Task<IActionResult> ForwardRequest(string url, HttpMethod method, object? body = null)
        {
            try
            {
                var request = new HttpRequestMessage(method, url);

                // Extrair claims do usuario autenticado e repassar como headers
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (!string.IsNullOrEmpty(username))
                {
                    request.Headers.Add("X-User-Name", username);
                }

                if (!string.IsNullOrEmpty(role))
                {
                    request.Headers.Add("X-User-Role", role);
                }

                // Adicionar body se houver
                if (body != null && (method == HttpMethod.Post || method == HttpMethod.Put))
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(body);
                    request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                }

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Gateway: {method} {url} -> {response.StatusCode} (User: {username}, Role: {role})");

                return StatusCode((int)response.StatusCode, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao rotear requisição para {url}");
                return StatusCode(500, new { error = "Erro no gateway ao processar requisição" });
            }
        }
    }
}