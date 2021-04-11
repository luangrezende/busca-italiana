using System.Threading.Tasks;

namespace BuscaImigrante
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var _imigranteService = new ImigranteService();

            await _imigranteService.StartAsync();
        }
    }
}
