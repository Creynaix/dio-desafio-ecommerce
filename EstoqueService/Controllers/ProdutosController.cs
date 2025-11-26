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

        public ProdutosController(EstoqueContext context)
        {
            _context = context;
        }

        // POST: api/produtos
        [HttpPost]
        public IActionResult CadastrarProduto([FromBody] Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ConsultarProduto), new { id = produto.Id }, produto);
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

        // PUT: api/produtos/{id}
        [HttpPut("{id}")]
        public IActionResult AtualizarEstoque(int id, [FromBody] Produto produtoAtualizado)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null)
            {
                return NotFound();
            }

            produto.Quantidade = produtoAtualizado.Quantidade;
            _context.SaveChanges();

            return NoContent();
        }
    }
}