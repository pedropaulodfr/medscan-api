using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using authentication_jwt.DTO.Cep;
using authentication_jwt.Models;
using Newtonsoft.Json;

namespace authentication_jwt.Services
{
    public class CepService
    {
        private readonly AppDbContext _dbContext;

        // Construtor para injetar o AppDbContext
        public CepService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ViaCepDTO> GetEndereco(string CEP)
        {
            string cepFormatado = CEP.Replace(" ", "").Replace("-", "");

            // Cria a requisição HTTP
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create($"http://viacep.com.br/ws/{cepFormatado}/json");
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        try
                        {
                            string result = await streamReader.ReadToEndAsync();
                            var endereco = JsonConvert.DeserializeObject<ViaCepDTO>(result);

                            return endereco;
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException("Problema ao desserializar o JSON: " + ex.Message, ex);
                        }
                    }
                }
                else
                {
                    throw new HttpRequestException($"Problema ao buscar o endereço: {response.StatusCode}");
                }
            }
        }

    }
}