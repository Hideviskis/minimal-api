using minimal_api.Dominio.Entidades;

namespace Test.Domain.Entidades;

[TestClass]
public sealed class AdmisnistradorTeste
{
    [TestMethod]
    public void TestarGetSetPropriedades()
    {  
        var adm = new Administrador();
        adm.Id = 1;
        adm.Email = "teste@teste.com";
        adm.Senha = "teste";
        adm.Perfil = "Adm";

        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual("teste@teste.com", adm.Email);
        Assert.AreEqual("teste", adm.Senha);
        Assert.AreEqual("Adm", adm.Perfil);


    }
}
