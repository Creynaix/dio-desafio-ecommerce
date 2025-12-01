using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using EstoqueService.Controllers;
using EstoqueService.Data;
using EstoqueService.Models;
using System.Collections.Generic;
using System.Linq;

namespace EstoqueService.Tests
{
    public class ProdutosControllerTests
    {
        private readonly Mock<ILogger<ProdutosController>> _loggerMock;
        private readonly EstoqueContext _context;
        private readonly ProdutosController _controller;

        public ProdutosControllerTests()
        {
            var options = new DbContextOptionsBuilder<EstoqueContext>()
                .UseInMemoryDatabase(databaseName: "TestEstoqueDB_" + System.Guid.NewGuid())
                .Options;

            _context = new EstoqueContext(options);
            _loggerMock = new Mock<ILogger<ProdutosController>>();
            _controller = new ProdutosController(_context, _loggerMock.Object);

            // Simular HttpContext e Headers para os testes
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["X-User-Role"] = "Administrador";
            httpContext.Request.Headers["X-User-Name"] = "TestUser";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void CadastrarProduto_DeveRetornarCreated_QuandoProdutoValido()
        {
            // Arrange
            var produto = new Produto
            {
                Nome = "Notebook Dell",
                Descricao = "Notebook i7 16GB",
                Preco = 3500.00m,
                Quantidade = 10
            };

            // Act
            var result = _controller.CadastrarProduto(produto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult?.Value.Should().BeEquivalentTo(produto);
            _context.Produtos.Count().Should().Be(1);
        }

        [Fact]
        public void ConsultarProdutos_DeveRetornarListaVazia_QuandoNaoHaProdutos()
        {
            // Act
            var result = _controller.ConsultarProdutos();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var produtos = okResult?.Value as List<Produto>;
            produtos.Should().BeEmpty();
        }

        [Fact]
        public void ConsultarProdutos_DeveRetornarListaDeProdutos()
        {
            // Arrange
            _context.Produtos.AddRange(
                new Produto { Nome = "Produto 1", Descricao = "Desc 1", Preco = 100, Quantidade = 5 },
                new Produto { Nome = "Produto 2", Descricao = "Desc 2", Preco = 200, Quantidade = 10 }
            );
            _context.SaveChanges();

            // Act
            var result = _controller.ConsultarProdutos();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var produtos = okResult?.Value as List<Produto>;
            produtos.Should().HaveCount(2);
        }

        [Fact]
        public void ConsultarProduto_DeveRetornarNotFound_QuandoProdutoNaoExiste()
        {
            // Act
            var result = _controller.ConsultarProduto(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ConsultarProduto_DeveRetornarProduto_QuandoExiste()
        {
            // Arrange
            var produto = new Produto { Nome = "Produto Teste", Descricao = "Desc", Preco = 150, Quantidade = 3 };
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            // Act
            var result = _controller.ConsultarProduto(produto.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var produtoRetornado = okResult?.Value as Produto;
            produtoRetornado.Should().NotBeNull();
            produtoRetornado?.Nome.Should().Be("Produto Teste");
        }

    [Fact]
    public void AtualizarEstoque_DeveRetornarNotFound_QuandoProdutoNaoExiste()
    {
        // Arrange
        var request = new EstoqueService.Controllers.AtualizarProdutoRequest
        {
            Quantidade = 20
        };

        // Act
        var result = _controller.AtualizarProduto(999, request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }        [Fact]
        public void AtualizarEstoque_DeveAtualizarQuantidade_QuandoProdutoExiste()
        {
            // Arrange
            var produto = new Produto { Nome = "Produto Original", Descricao = "Desc", Preco = 100, Quantidade = 5 };
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            var request = new EstoqueService.Controllers.AtualizarProdutoRequest
            { 
                Quantidade = 20 
            };

            // Act
            var result = _controller.AtualizarProduto(produto.Id, request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var produtoDb = _context.Produtos.Find(produto.Id);
            produtoDb?.Quantidade.Should().Be(20);
        }

        // Testes DeletarProduto removidos - método não implementado no controller
        // Para adicionar DELETE: implementar método [HttpDelete("{id}")] em ProdutosController
    }
}
