using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using VendasService.Controllers;
using VendasService.Data;
using VendasService.Models;
using VendasService.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendasService.Tests
{
    public class PedidosControllerTests
    {
        private readonly Mock<ILogger<PedidosController>> _loggerMock;
        private readonly Mock<IRabbitMQProducer> _rabbitMQProducerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly VendasContext _context;
        private readonly PedidosController _controller;

        public PedidosControllerTests()
        {
            var options = new DbContextOptionsBuilder<VendasContext>()
                .UseInMemoryDatabase(databaseName: "TestVendasDB_" + System.Guid.NewGuid())
                .Options;

            _context = new VendasContext(options);
            _loggerMock = new Mock<ILogger<PedidosController>>();
            _rabbitMQProducerMock = new Mock<IRabbitMQProducer>();
            _httpClient = new HttpClient(); // HttpClient não pode ser mockado facilmente, usar instância real
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(c => c["EstoqueServiceUrl"]).Returns("http://localhost:5000");

            _controller = new PedidosController(
                _context,
                _rabbitMQProducerMock.Object,
                _httpClient,
                _configurationMock.Object,
                _loggerMock.Object
            );

            // Simular HttpContext e Headers para os testes
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["X-User-Role"] = "Cliente";
            httpContext.Request.Headers["X-User-Name"] = "TestUser";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task ConsultarPedidos_DeveRetornarListaVazia_QuandoNaoHaPedidos()
        {
            // Act
            var result = await _controller.ConsultarPedidos();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var pedidos = okResult?.Value as List<Pedido>;
            pedidos.Should().BeEmpty();
        }

        [Fact]
        public async Task ConsultarPedidos_DeveRetornarListaDePedidos()
        {
            // Arrange
            var pedido1 = new Pedido
            {
                Cliente = "Cliente 1",
                Data = System.DateTime.Now,
                Status = "Confirmado",
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { ProdutoId = 1, Quantidade = 2 }
                }
            };

            var pedido2 = new Pedido
            {
                Cliente = "Cliente 2",
                Data = System.DateTime.Now,
                Status = "Pendente",
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { ProdutoId = 2, Quantidade = 1 }
                }
            };

            _context.Pedidos.AddRange(pedido1, pedido2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.ConsultarPedidos();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var pedidos = okResult?.Value as List<Pedido>;
            pedidos.Should().HaveCount(2);
        }

        [Fact]
        public async Task ConsultarPedido_DeveRetornarNotFound_QuandoPedidoNaoExiste()
        {
            // Act
            var result = await _controller.ConsultarPedido(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ConsultarPedido_DeveRetornarPedido_QuandoExiste()
        {
            // Arrange
            var pedido = new Pedido
            {
                Cliente = "Cliente Teste",
                Data = System.DateTime.Now,
                Status = "Confirmado",
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { ProdutoId = 1, Quantidade = 3 }
                }
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.ConsultarPedido(pedido.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var pedidoRetornado = okResult?.Value as Pedido;
            pedidoRetornado.Should().NotBeNull();
            pedidoRetornado?.Cliente.Should().Be("Cliente Teste");
            pedidoRetornado?.Itens.Should().HaveCount(1);
        }

        [Fact]
        public async Task CriarPedido_DeveRetornarBadRequest_QuandoPedidoSemItens()
        {
            // Arrange
            var pedido = new Pedido
            {
                Cliente = "Cliente Sem Itens",
                Data = System.DateTime.Now,
                Status = "Pendente",
                Itens = null!
            };

            // Act
            var result = await _controller.CriarPedido(pedido);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult?.Value.Should().Be("O pedido deve conter pelo menos um item");
        }

        [Fact]
        public async Task CriarPedido_DeveRetornarBadRequest_QuandoQuantidadeInvalida()
        {
            // Arrange
            var pedido = new Pedido
            {
                Cliente = "Cliente Teste",
                Data = System.DateTime.Now,
                Status = "Pendente",
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { ProdutoId = 1, Quantidade = 0 }
                }
            };

            // Act
            var result = await _controller.CriarPedido(pedido);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
