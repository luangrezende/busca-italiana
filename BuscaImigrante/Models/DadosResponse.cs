using System;
using System.Collections.Generic;
using System.Text;

namespace BuscaImigrante.Models
{
    public class DadosResponse
    {
        public string id_hospedaria_pk { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string idade { get; set; }
        public string num_familia { get; set; }
        public string dt_chegada { get; set; }
        public string parentesco { get; set; }
        public string nacionalidade { get; set; }
        public string num_livro { get; set; }
        public string num_pag { get; set; }
    }
}
