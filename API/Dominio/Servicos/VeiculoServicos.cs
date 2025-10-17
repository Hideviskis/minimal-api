using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;
using minimal_api.DTO;
using minimal_api.Infraestrutura.Db;
using minimal_api.Infraestrutura.Interfaces;

namespace minimal_api.Dominio.Servicos
{
    public class VeiculoServicos : IVeiculoServicos
    {
        private readonly DbContexto _contexto;
        public VeiculoServicos(DbContexto contexto)
        {
            _contexto = contexto;
        }

        public void Apagar(Veiculo veiculo)
        {
            _contexto.Veiculos.Remove(veiculo);
            _contexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }

        public Veiculo? BuscarId(int id)
        {
            return _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Incluir(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
        }

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            var query = _contexto.Veiculos.AsQueryable();
            int itensPorPagina = 10;

            
            if (pagina.HasValue && pagina.Value > 0)
            {
                int paginaValida = pagina.Value;
                int offset = (paginaValida - 1) * itensPorPagina;
        
                query = query.Skip(offset).Take(itensPorPagina);
            }
            else
            {
                
                query = query.Skip(0).Take(itensPorPagina);
            }       
    
            return query.ToList();
            
        }
    }
}