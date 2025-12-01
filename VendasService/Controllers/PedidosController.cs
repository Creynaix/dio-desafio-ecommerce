using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendasService.Data;
using VendasService.Models;
using VendasService.Services;
using System.Net.Http;
using System.Text.Json;

namespace VendasService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly VendasContext _context;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PedidosController> _logger;

        public PedidosController(
            VendasContext context, 
            IRabbitMQProducer rabbitMQProducer, 
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<PedidosController> logger)
        {
            _context = context;
            _rabbitMQProducer = rabbitMQProducer;
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        // POST: api/pedidos (Recebe X-User-Name e X-User-Role do Gateway)
        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] Pedido pedido)
        {
            // Validação básica
            if (pedido.Itens == null || !pedido.Itens.Any())
            {
                return BadRequest("O pedido deve conter pelo menos um item");
            }

            // Validação básica de quantidade
            foreach (var item in pedido.Itens)
            {
                if (item.Quantidade <= 0)
                {
                    return BadRequest($"Quantidade inválida para o produto {item.ProdutoId}");
                }
            }

            // TRUSTED SUBSYSTEM PATTERN: Gateway já validou autenticação e autorização
            // Estoque será atualizado de forma assíncrona via RabbitMQ após criação do pedido

            // Criar pedido
            pedido.Data = DateTime.Now;
            pedido.Status = "Confirmado";

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // Enviar mensagem estruturada para RabbitMQ
            var vendaMessage = new VendaMessage
            {
                PedidoId = pedido.Id,
                Itens = pedido.Itens.Select(i => new ItemVendaMessage
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade
                }).ToList()
            };

            _rabbitMQProducer.SendMessage(vendaMessage);
            _logger.LogInformation($"Pedido {pedido.Id} criado e mensagem enviada ao RabbitMQ");

            return CreatedAtAction(nameof(ConsultarPedido), new { id = pedido.Id }, pedido);
        }

        // GET: api/pedidos
        [HttpGet]
        public async Task<IActionResult> ConsultarPedidos()
        {
            var userName = Request.Headers["X-User-Name"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            
            _logger.LogInformation($"ConsultarPedidos - User: {userName}, Role: {userRole}");
            
            var pedidos = await _context.Pedidos.Include(p => p.Itens).ToListAsync();
            return Ok(pedidos);
        }

        // GET: api/pedidos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ConsultarPedido(int id)
        {
            var pedido = await _context.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }
    }

    // DTO para deserializar resposta do EstoqueService
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
    }
}