using EstoqueService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Banco
builder.Services.AddDbContext<EstoqueContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// RabbitMQ Consumer como Background Service
builder.Services.AddHostedService<EstoqueService.Services.RabbitMQConsumer>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Controllers com JSON case-insensitive
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplicar migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EstoqueContext>();
        // Usar EnsureCreated para garantir que tabelas sejam criadas
        var created = context.Database.EnsureCreated();
        if (created)
        {
            Console.WriteLine("✅ Banco de dados EstoqueDB criado com sucesso");
        }
        else
        {
            Console.WriteLine("ℹ️ Banco de dados EstoqueDB já existe");
        }

        // Seed de produtos iniciais (apenas se banco estiver vazio)
        if (!context.Produtos.Any())
        {
            Console.WriteLine("📦 Cadastrando produtos iniciais...");
            
            context.Produtos.AddRange(
                new EstoqueService.Models.Produto 
                { 
                    Nome = "Notebook Dell Inspiron 15", 
                    Descricao = "Intel i7 11ª geração, 16GB RAM, 512GB SSD", 
                    Preco = 4299.90m, 
                    Quantidade = 15 
                },
                new EstoqueService.Models.Produto 
                { 
                    Nome = "Mouse Gamer Logitech G502", 
                    Descricao = "RGB, 16000 DPI, 11 botões programáveis", 
                    Preco = 249.90m, 
                    Quantidade = 50 
                },
                new EstoqueService.Models.Produto 
                { 
                    Nome = "Teclado Mecânico Razer BlackWidow", 
                    Descricao = "Switch Green, RGB Chroma, Layout ABNT2", 
                    Preco = 799.90m, 
                    Quantidade = 30 
                },
                new EstoqueService.Models.Produto 
                { 
                    Nome = "Monitor LG UltraWide 29\"", 
                    Descricao = "IPS Full HD, 75Hz, FreeSync, HDMI", 
                    Preco = 1199.00m, 
                    Quantidade = 20 
                },
                new EstoqueService.Models.Produto 
                { 
                    Nome = "Headset HyperX Cloud II", 
                    Descricao = "7.1 Surround, Microfone removível, USB", 
                    Preco = 499.90m, 
                    Quantidade = 40 
                },
                new EstoqueService.Models.Produto 
                { 
                    Nome = "Webcam Logitech C920 HD Pro", 
                    Descricao = "Full HD 1080p, Microfone estéreo, Autofoco", 
                    Preco = 399.90m, 
                    Quantidade = 25 
                },
                new EstoqueService.Models.Produto 
                { 
                    Nome = "SSD Kingston A400 480GB", 
                    Descricao = "SATA III, Leitura 500MB/s, Gravação 450MB/s", 
                    Preco = 279.90m, 
                    Quantidade = 60 
                },
                new EstoqueService.Models.Produto 
                { 
                    Nome = "Memória RAM Corsair Vengeance 16GB", 
                    Descricao = "DDR4 3200MHz, RGB, Kit 2x8GB", 
                    Preco = 449.90m, 
                    Quantidade = 35 
                },
                new EstoqueService.Models.Produto 
                { 
                    Nome = "Placa de Vídeo RTX 3060 Ti", 
                    Descricao = "8GB GDDR6, Ray Tracing, DLSS", 
                    Preco = 2899.00m, 
                    Quantidade = 8 
                },
                new EstoqueService.Models.Produto 
                { 
                    Nome = "Cadeira Gamer DXRacer Formula", 
                    Descricao = "Reclinável 135°, Apoio lombar, Couro PU", 
                    Preco = 1599.90m, 
                    Quantidade = 12 
                }
            );

            context.SaveChanges();
            Console.WriteLine("✅ 10 produtos cadastrados com sucesso!");
        }
        else
        {
            Console.WriteLine($"ℹ️ Banco já contém {context.Produtos.Count()} produtos");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao criar banco: {ex.Message}");
    }
}

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
