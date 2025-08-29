using EcommerceApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Registrar o DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EcommerceDb"));

// Registrar controllers
builder.Services.AddControllers();

// Registrar o Swagger com suporte a anotações e XML
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); // Habilita uso de [SwaggerOperation], etc.

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de E-commerce",
        Version = "v1",
        Description = "Documentação da API de E-commerce com Swagger"
    });

    // Incluir comentários XML (se configurado no .csproj)
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// Configurar a pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-commerce API v1");
        c.RoutePrefix = string.Empty; // Abre Swagger diretamente na raiz
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();