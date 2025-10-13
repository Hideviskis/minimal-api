using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.Dominio.ModelViews
{
    public class Home
    {
        public string Documentacao { get => "/swagger"; }
        public string Mensagem { get => "Bem Vindo a API de veiculos - Minimal API"; }
    }
}