using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OperationGenerator.APIs
{
    class ExchangeRateAPI
    {
        const string apiKey = "e7b5edbe21c14dab8632fe4d";
        const string getDevisesUrl = $"https://v6.exchangerate-api.com/v6/{apiKey}/codes";
        public static async Task<List<string>>? ObtenirCodesDevises()
        {
            List<string> codes = new List<string>();
            try
            {
                
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(getDevisesUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

                if (result?.supported_codes != null)
                {
                    foreach (var currency in result.supported_codes)
                    {
                        codes.Add(currency[0]);

                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
            return codes;
        }
        class ApiResponse
        {
            public List<string[]>? supported_codes { get; set; }
        }

    }
}