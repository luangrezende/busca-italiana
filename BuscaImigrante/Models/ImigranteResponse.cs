using System.Collections.Generic;

namespace BuscaImigrante.Models
{
    public class ImigranteResponse
    {
        public int page { get; set; }
        public int limitPage { get; set; }
        public List<DadosResponse> dados { get; set; }
    }
}
