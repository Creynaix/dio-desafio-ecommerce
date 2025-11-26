using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public GatewayController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Roteamento para o Microserviço de Vendas
        [HttpGet("vendas/{*path}")]
        public async Task<IActionResult> RoteamentoVendas(string path)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5000/{path}");
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        // Roteamento para o Microserviço de Estoque
        [HttpGet("estoque/{*path}")]
        public async Task<IActionResult> RoteamentoEstoque(string path)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5002/{path}");
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}