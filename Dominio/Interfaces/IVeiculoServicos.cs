using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;
using minimal_api.DTO;

namespace minimal_api.Infraestrutura.Interfaces
{
    public interface IVeiculoServicos
    {
        List<Veiculo> Todos(int? pagina, string? nome = null, string? marca = null);
        Veiculo? BuscarId(int id);
        void Incluir(Veiculo veiculo );
        void Atualizar(Veiculo veiculo);
        void Apagar(Veiculo veiculo);
    }
}