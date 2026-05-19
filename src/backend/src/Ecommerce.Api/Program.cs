using Ecommerce.Modules.Catalog;
using Ecommerce.Modules.Catalog.Endpoints;
using Ecommerce.Modules.Inventory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddInventoryModule();
builder.Services.AddCatalogModule();

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => Results.Ok(new { service = "Ecommerce API", module = "Catalog" }));
app.MapCatalogEndpoints();

app.Run();

public partial class Program;
