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
        const string hostApiKey = "514b07ba3297e36d6affaa7e5c2a738a";
        const string getDevisesUrl = $"https://v6.exchangerate-api.com/v6/{apiKey}/codes";
        public const string baseRate = "EUR";
        public static async Task<List<string>>? ObtenirCodesDevises()
        {
            List<string> codes = new List<string>();
            try
            {
                
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(getDevisesUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GetTauxChangeResponse>(jsonResponse);

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

        public static async Task<Dictionary<string, decimal>?> ObtenirTauxChange(List<string> devises, DateTime date)
        {
            string url = $"https://api.exchangerate.host/historical?access_key={hostApiKey}&date={date.ToString("yyyy-MM-dd")}&source={baseRate}&currencies={string.Join(",", devises)}";
            try
            {
                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ExchangeRateResponse>(jsonResponse);
                Console.WriteLine($"Réponse brute : {jsonResponse}");

                if (result?.quotes != null)
                {
                    Dictionary<string, decimal> tauxDevises = new Dictionary<string, decimal>();
                    foreach (var devise in devises)
                    {
                        string key = $"{baseRate}{devise}";

                        if (result.quotes.ContainsKey(key))
                        {
                            tauxDevises.Add(devise, result.quotes[key]);
                        }
                        else
                        {
                            Console.WriteLine($"Clé {key} non trouvée dans les taux.");
                            tauxDevises.Add(devise, 0);
                        }
                    }
                    return tauxDevises;
                }
                else
                {
                    Console.WriteLine("Aucune donnée de taux trouvée dans la réponse.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erreur de connexion à l'API : {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur générale : {ex.Message}");
            }

            return null;
        }
        class GetTauxChangeResponse
        {
            public List<string[]>? supported_codes { get; set; }
        }
        class ExchangeRateResponse
        {
            public Dictionary<string, decimal>? quotes { get; set; }
        }
    }
}