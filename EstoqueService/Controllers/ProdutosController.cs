using Microsoft.AspNetCore.Mvc;
using EstoqueService.Data;
using EstoqueService.Models;

namespace EstoqueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly EstoqueContext _context;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(EstoqueContext context, ILogger<ProdutosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/produtos (Recebe X-User-Role do Gateway)
        [HttpPost]
        public IActionResult CadastrarProduto([FromBody] Produto produto)
        {
            var userRole = Request.Headers["X-User-Role"].ToString();
            var userName = Request.Headers["X-User-Name"].ToString();
            
            _logger.LogInformation($"CadastrarProduto - User: {userName}, Role: {userRole}");
            
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ConsultarProduto), new { id = produto.Id }, produto);
        }

        // PUT: api/produtos/{id} (Recebe X-User-Role do Gateway)
        [HttpPut("{id}")]
        public IActionResult AtualizarProduto(int id, [FromBody] AtualizarProdutoRequest request)
        {
            var userRole = Request.Headers["X-User-Role"].ToString();
            var userName = Request.Headers["X-User-Name"].ToString();
            
            _logger.LogInformation($"AtualizarProduto - User: {userName}, Role: {userRole}, ProdutoId: {id}");
            
            var produto = _context.Produtos.Find(id);
            if (produto == null)
            {
                return NotFound(new { mensagem = $"Produto com ID {id} não encontrado" });
            }

            // Atualizar apenas os campos fornecidos (não nulos)
            if (!string.IsNullOrWhiteSpace(request.Nome))
                produto.Nome = request.Nome;
            
            if (!string.IsNullOrWhiteSpace(request.Descricao))
                produto.Descricao = request.Descricao;
            
            if (request.Preco.HasValue && request.Preco.Value > 0)
                produto.Preco = request.Preco.Value;
            
            if (request.Quantidade.HasValue)
                produto.Quantidade = request.Quantidade.Value;

            _context.SaveChanges();
            
            _logger.LogInformation($"Produto {id} atualizado com sucesso");
            return Ok(produto);
        }

        // GET: api/produtos
        [HttpGet]
        public IActionResult ConsultarProdutos()
        {
            var produtos = _context.Produtos.ToList();
            return Ok(produtos);
        }

        // GET: api/produtos/{id}
        [HttpGet("{id}")]
        public IActionResult ConsultarProduto(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }
    }

    // DTO para atualização de produto (campos opcionais)
    public class AtualizarProdutoRequest
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal? Preco { get; set; }
        public int? Quantidade { get; set; }
    }
}