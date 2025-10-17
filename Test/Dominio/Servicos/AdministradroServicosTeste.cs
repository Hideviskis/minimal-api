using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;

namespace Test.Domain.Entidades;

[TestClass]
public class AdministradorServicoTest
{
    private DbContexto CriarContextoDeTeste()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DbContexto(configuration);
    }
[TestMethod]
public void TestandoSalvarAdministrador()
{
    var context = CriarContextoDeTeste();
    context.Database.ExecuteSqlRaw("DELETE FROM Administradores");
    context.Database.ExecuteSqlRaw("ALTER TABLE Administradores AUTO_INCREMENT = 1;");

    var adm = new Administrador
    {
        Email = "teste@teste.com",
        Senha = "teste",
        Perfil = "Adm"
    };

    var administradorServico = new AdministradorServicos(context);

    // Act
    administradorServico.Incluir(adm);

    // Recupera do banco
    var todos = administradorServico.Todos(1).ToList();

    // Assert
    Assert.AreEqual(1, todos.Count);              
    Assert.AreEqual(adm.Email, todos[0].Email);  
}

}

//     [TestMethod]
//     public void TestandoSalvarAdministrador()
//     {
//         // Arrange
//         var context = CriarContextoDeTeste();
//         context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

//         var adm = new Administrador();
//         adm.Email = "teste@teste.com";
//         adm.Senha = "teste";
//         adm.Perfil = "Adm";

//         var administradorServico = new AdministradorServicos(context);

//         // Act
//         administradorServico.Incluir(adm);

//         // Assert
//         Assert.AreEqual(1, administradorServico.Todos(1).Count());
//     }

//     [TestMethod]
//     public void TestandoBuscaPorId()
//     {
//         // Arrange
//         var context = CriarContextoDeTeste();
//         context.Database.ExecuteSqlRaw("DELETE FROM Administradores");
//         context.Database.ExecuteSqlRaw("ALTER TABLE Administradores AUTO_INCREMENT = 1;");


//         var adm = new Administrador();
//         adm.Email = "teste@teste.com";
//         adm.Senha = "teste";
//         adm.Perfil = "Adm";

//         var administradorServico = new AdministradorServicos(context);

//         // Act
//         administradorServico.Incluir(adm);
//         var admDoBanco = administradorServico.BuscarId(adm.Id);

//         // Assert
//         Assert.AreEqual(1, admDoBanco?.Id);
//     }
// }