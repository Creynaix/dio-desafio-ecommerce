using Microsoft.AspNetCore.Mvc;
using VendasService.Data;
using VendasService.Models;

namespace VendasService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly VendasContext _context;

        public PedidosController(VendasContext context)
        {
            _context = context;
        }

        // POST: api/pedidos
        [HttpPost]
        public IActionResult CriarPedido([FromBody] Pedido pedido)
        {
            // Validação do estoque (simulação)
            foreach (var item in pedido.Itens)
            {
                // Simular validação do estoque
                if (item.Quantidade <= 0)
                {
                    return BadRequest($"Quantidade inválida para o produto {item.ProdutoId}");
                }
            }

            pedido.Data = DateTime.Now;
            pedido.Status = "Confirmado";

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ConsultarPedido), new { id = pedido.Id }, pedido);
        }

        // GET: api/pedidos
        [HttpGet]
        public IActionResult ConsultarPedidos()
        {
            var pedidos = _context.Pedidos.ToList();
            return Ok(pedidos);
        }

        // GET: api/pedidos/{id}
        [HttpGet("{id}")]
        public IActionResult ConsultarPedido(int id)
        {
            var pedido = _context.Pedidos.Find(id);
            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }
    }
}