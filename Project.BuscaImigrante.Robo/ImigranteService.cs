using BuscaImigrante.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuscaImigrante
{
    public class ImigranteService
    {
        public async Task StartAsync()
        {
            Console.WriteLine("Iniciando busca de imigrantes...");
            await SearchImigrante();
            Console.WriteLine("Busca de imigrantes finalizada!");
        }

        public async Task SearchImigrante()
        {
            PesquisaResponse Imigrantes = new PesquisaResponse();
            ImigranteResponse imigranteResponse = new ImigranteResponse
            {
                page = 1,
                limitPage = 5,
                dados = new List<DadosResponse>()
            };

            Filters filters = new Filters
            {
                Age = "38",
                Name = "giuseppe",
                Surname = "",
                Nation = "ita"
            };

            while (imigranteResponse.page <= imigranteResponse.limitPage)
            {
                string response = await GetAllImigrantesAsync(imigranteResponse.page);

                if (Imigrantes.dados == null)
                {
                    Imigrantes = FormatResponseToImigrantes(response);
                    imigranteResponse.limitPage = Imigrantes.total_paginas;
                }
                else
                {
                    var responseFormated = FormatResponseToImigrantes(response);
                    Imigrantes.dados = responseFormated.dados;
                    Imigrantes.pagina = responseFormated.pagina;
                }

                await FindImigrantes(Imigrantes, imigranteResponse, filters);
            }
        }

        public async Task<string> GetAllImigrantesAsync(int page)
        {
            using var httpClient = new HttpClient();
            string Url = $"http://www.arquivoestado.sp.gov.br/site/acervo/memoria_do_imigrante/pesquisa_memimig_hospedaria";

            IList<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("frm", "ano_pesq=1897&vapor_pesq=les+andes") },
                { new KeyValuePair<string, string>("page", page.ToString()) },
                { new KeyValuePair<string, string>("limit", "20") },
            };

            var response = await httpClient.PostAsync(Url, new FormUrlEncodedContent(nameValueCollection));
            return await response.Content.ReadAsStringAsync();
        }

        public PesquisaResponse FormatResponseToImigrantes(string response)
        {
            return JsonSerializer.Deserialize<PesquisaResponse>(response);
        }

        public bool FilterDadosResponse(DadosResponse dadosResponse, Filters filters)
        {
            return (dadosResponse.nome == null || dadosResponse.nome.ToLower().Contains(filters.Name.ToLower()))
                 && (dadosResponse.sobrenome == null || dadosResponse.sobrenome.ToLower().Contains(filters.Surname.ToLower()))
                 && (dadosResponse.idade == null || dadosResponse.idade.ToLower().Contains(filters.Age.ToLower()))
                 && (dadosResponse.nacionalidade == null || dadosResponse.nacionalidade.ToLower().Contains(filters.Nation.ToLower()));
        }
        
        public void PrintImigrante(DadosResponse dadosResponse)
        {
            Console.WriteLine($"Nome: {dadosResponse.nome}");
            Console.WriteLine($"Sobrenome: {dadosResponse.sobrenome}");
            Console.WriteLine($"Idade: {dadosResponse.idade}");
            Console.WriteLine($"Data chegada: {dadosResponse.dt_chegada}");
            Console.WriteLine($"Nacionalidade: {dadosResponse.nacionalidade}");
            Console.WriteLine($"Livro: {dadosResponse.num_livro}");
            Console.WriteLine($"Pagina: {dadosResponse.num_pag}");
            Console.WriteLine($".............................................");
        }

        public async Task<ImigranteResponse> FindImigrantes(PesquisaResponse pesquisaResponse, ImigranteResponse imigranteResponse, Filters filters)
        {
            int page = Convert.ToInt32(pesquisaResponse.pagina) + 1;
            imigranteResponse.page = page;

            foreach (var imigrante in pesquisaResponse.dados)
            {
                if (FilterDadosResponse(imigrante, filters))
                {
                    imigranteResponse.dados.Add(imigrante);
                    PrintImigrante(imigrante);
                }
            }
            return imigranteResponse;
        }
    }
}
