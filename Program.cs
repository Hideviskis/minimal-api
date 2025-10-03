using Microsoft.EntityFrameworkCore;
using minimal_api.DTO;
using minimal_api.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/Login", (minimal_api.DTO.LoginDTO loginDTO) =>{   
 
    if (loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456")
    {
        return Results.Ok("Login com Sucesso");
    }   
    else
    {
        return Results.Unauthorized();
    }
});
app.Run();
