using VendasService.Data;
using VendasService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen();

// Configurar o banco de dados
builder.Services.AddDbContext<VendasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar RabbitMQProducer (usando interface para testabilidade)
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();

// Registrar HttpClient para chamar EstoqueService
builder.Services.AddHttpClient();

var app = builder.Build();

// Aplicar migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<VendasContext>();
        // Usar EnsureCreated para garantir que tabelas sejam criadas
        var created = context.Database.EnsureCreated();
        if (created)
        {
            Console.WriteLine("✅ Banco de dados VendasDB criado com sucesso");
        }
        else
        {
            Console.WriteLine("ℹ️ Banco de dados VendasDB já existe");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao criar banco: {ex.Message}");
    }
}

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
