using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTO.Enuns;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.ModelViews;
using minimal_api.Dominio.Servicos;
using minimal_api.DTO;
using minimal_api.Infraestrutura.Db;
using minimal_api.Infraestrutura.Interfaces;
using minimal_api.Migrations;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.ConfigureHttpJsonOptions(options =>
// {
//     options.SerializerOptions.WriteIndented = true;
// });


#region Builder
builder.Services.AddScoped<IAdministradorServicos, AdministradorServicos>();
builder.Services.AddScoped<IVeiculoServicos, VeiculoServicos>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
#endregion

#region  Home
app.MapGet("/", () =>Results.Json( new Home())).WithTags("Home");
#endregion 

#region  Administradores

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServicos administradorServicos) =>
{

    if (administradorServicos.Login(loginDTO) != null)
    {
        return Results.Ok("Login com Sucesso");
    }
    else
    {
        return Results.Unauthorized();
    }

}).WithTags("Administradores");
app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServicos administradorServicos) =>
{
    return Results.Ok("administradorServicos.Todos(pagina)");

}).WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministradorServicos administradorServicos) =>
{
    var administrador = administradorServicos.BuscarID(id);
    if (administrador == null) return Results.NotFound();
    return Results.Ok(administrador);
}).WithTags("Veiculos");

app.MapPost("/administradores", ([FromBody]  AdministradorDTO administradorDTO, IAdministradorServicos administradorServicos) =>
{
    var validacao = new ErrosDeValidacao()
    {
        Mensagem = new List<string>()
    };

    if (string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagem.Add("Email nao pode estar vazio");

    if (string.IsNullOrEmpty(administradorDTO.Senha))

        validacao.Mensagem.Add("Senha nao pode estar vazia");

    if (administradorDTO.Perfil == null)
        validacao.Mensagem.Add("Perfil nao pode estar vazio");
    if (validacao.Mensagem.Count > 0)
        return Results.BadRequest(validacao);

        var veiculo = new Administrador
        {
            Email = administradorDTO.Email,
            Senha = administradorDTO.Senha,
            Perfil = administradorDTO.Perfil.ToString() ?? Perfil.editor.ToString()
        };
        administradorServicos.Incluir(veiculo);
         return Results.Created($"/administradore/{veiculo.Id}", veiculo);
   
}).WithTags("Administradores");
#endregion

#region Veiculos
ErrosDeValidacao validacaoDTO(VeiculoDTO veiculoDTO) {
    var validacao = new ErrosDeValidacao()
    {
        Mensagem = new List<string>(),

    };
    
    if (string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagem.Add("O nome nao pode ser Vazio");
    
    if (string.IsNullOrEmpty(veiculoDTO.Marca))
    
        validacao.Mensagem.Add("A Marca nao pode ficar em branco ");

    if (veiculoDTO.Ano < 1950)

        validacao.Mensagem.Add("Veiculo Muito antigo veiculos perimitido somente acima do ano de 1950");
        
    return validacao;
    
}
app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServicos veiculoServicos) =>
{
    var validacao = validacaoDTO(veiculoDTO);
    if (validacao.Mensagem.Count > 0)

        return Results.BadRequest(validacao);
    
    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };
    veiculoServicos.Incluir(veiculo);
    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoServicos veiculoServicos) =>
{
    var veiculos = veiculoServicos.Todos(pagina);
    return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoServicos veiculoServicos) =>
{
    var veiculos = veiculoServicos.BuscarId(id);
    if (veiculos == null) return Results.NotFound();
    return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoServicos veiculoServicos) =>
{
    var veiculos = veiculoServicos.BuscarId(id);
    if (veiculos == null) return Results.NotFound();
    
    var validacao = validacaoDTO(veiculoDTO);
    if (validacao.Mensagem.Count > 0)
        return Results.BadRequest(validacao);

    veiculos.Nome = veiculoDTO.Nome;
    veiculos.Marca = veiculoDTO.Marca;
    veiculos.Ano = veiculoDTO.Ano;

    veiculoServicos.Atualizar(veiculos);

    return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoServicos veiculoServicos) =>
{
    var veiculos = veiculoServicos.BuscarId(id);
    if (veiculos == null) return Results.NotFound();

    veiculoServicos.Apagar(veiculos);
    
    return Results.NoContent();
}).WithTags("Veiculos");


#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion