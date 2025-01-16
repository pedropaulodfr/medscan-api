using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace authentication_jwt.Utils
{
    public class Funcoes
    {
        public static string GerarSenhaAleatoria()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(caracteres, 8)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GerarHashSHA256(string _input)
        {
            string input = string.IsNullOrWhiteSpace(_input) ? GerarSenhaAleatoria() : _input;
            using (SHA256 sha256 = SHA256.Create())
            {
                // Converter a string de entrada para bytes
                byte[] bytes = Encoding.UTF8.GetBytes(input);

                // Computar o hash
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Converter o hash para uma string hexadecimal
                StringBuilder hashHex = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashHex.Append(b.ToString("x2")); // "x2" para formato hexadecimal com dois dígitos
                }

                return hashHex.ToString();
            }
        }
        public async Task<string> UploadImagem(string imagemBase64)
        {
            try
            {
                string apiKey = "a3c9d1f0c861c8f5326f1a70e450cfa4";  // Substitua com sua chave de API do ImgBB
                string url = "https://api.imgbb.com/1/upload"; // Certifique-se de que este preset está configurado corretamente no painel do Cloudinary

                // Converte o Base64 em bytes
                byte[] imagemBytes = Convert.FromBase64String(imagemBase64);

                using (var client = new HttpClient())
                {
                    // Cria o formulário multipart
                    var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(apiKey), "key");
                    formData.Add(new StringContent(imagemBase64), "image"); 

                    // Envia a requisição POST
                    var response = await client.PostAsync(url, formData);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse o JSON de resposta para obter a URL da imagem
                        dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                        string imageUrl = jsonResponse.data.url;  // URL da imagem carregada

                        // Exibe a URL da imagem
                        Console.WriteLine($"Imagem carregada com sucesso! URL: {imageUrl}");
                        return imageUrl;
                    }
                    else
                    {
                        // Exibe o erro retornado pela API
                        Console.WriteLine($"Erro ao enviar a imagem: {result}");
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao fazer upload: {ex.Message}");
                return "";
            }
        }

    }
}