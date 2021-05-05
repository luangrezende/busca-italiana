using System.Collections.Generic;

namespace BuscaImigrante.Models
{
    public class PesquisaResponse
    {
        public bool status { get; set; }
        public string msg { get; set; }
        public string pagina { get; set; }
        public int total_paginas { get; set; }
        public string total_registro { get; set; }
        public IEnumerable<DadosResponse> dados { get; set; }
    }
}
