using OperationGenerator.APIs;
using System.Text.Json;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OperationGenerator
{
   
    public class OperationGen
    {
        const string baseCardNumber = "497401850223";


        static async Task Main()
        {
            DateTime date;
            date = DateTime.Now;

            List<Operation> operations = await GenererOperations(10);
            GenererCsv(operations);
            operations = VerifyTransactionsFromCsv($"C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\OperationGenerator\\Transactions\\operations-{date.DayOfYear}.csv");
           // await AjoutTauxChange(operations);
            //Console.WriteLine("testttttt");
           // GenerateDailyJson(operations);
        }
        

        static async Task<List<Operation>> GenererOperations(int count)
        {
            var operations = new List<Operation>();
            var random = new Random();
            List<string> codes = await ExchangeRateAPI.ObtenirCodesDevises();

            for (int i = 0; i < count;)
            {
                string cardNumber = baseCardNumber + random.Next(1000, 1042).ToString();
                if (i % 2 == 0 && !IsValidLuhn(cardNumber))
                    continue;

                operations.Add(new Operation
                {
                    NumCarte = cardNumber,
                    Montant = random.Next(100, 10000),
                    OperationType = (OperationType)random.Next(Enum.GetValues(typeof(OperationType)).Length),
                    Date = DateTime.Now,
                    Devise = codes[random.Next(codes.Count) - 1]   
                });

                i++;
            }
            return operations;
        }
        public static bool IsValidLuhn(string number)
        {
            int sum = 0;
            bool alternate = false;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digit = number[i] - '0';
                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9) digit -= 9;
                }
                sum += digit;
                alternate = !alternate;
            }
            return (sum % 10 == 0);
        }

        static void GenererCsv(List<Operation> operations)
        {
            DateTime date;
            date = DateTime.Now;
            using (var writer = new StreamWriter($"C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\OperationGenerator\\Transactions\\operations-{date.DayOfYear}.csv"))
            {
                
                writer.WriteLine("NumCarte,Montant,OperationType,Date,Devise");
                foreach (var operation in operations)
                {
                    writer.WriteLine($"{operation.NumCarte},{operation.Montant},{operation.OperationType},{operation.Date:yyyy-MM-dd HH:mm:ss},{operation.Devise}");
                }
            }
        }

        public static List<Operation> VerifyTransactionsFromCsv(string filePath)
        {
            DateTime date;
            date = DateTime.Now;
            var invalidTransactions = new List<string>();
            List<Operation> operations = new List<Operation> { };

            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine(); // Lire l'en-tête
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    string cardNumber = values[0];
                    if (values.Length < 5 || !IsValidLuhn(cardNumber))
                    {
                        invalidTransactions.Add(line);
                    }
                    else
                    {
                        operations.Add(new Operation
                        {
                            NumCarte = values[0],
                            Montant = double.Parse(values[1]),
                            OperationType = Enum.Parse<OperationType>(values[2]),
                            Date = DateTime.Parse(values[3]),
                            Devise = values[4]
                        });
                    }
                }
                
            }

            if (invalidTransactions.Count > 0)
            {
                File.WriteAllLines($"C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\OperationGenerator\\Transactions\\invalid_transactions-{date.DayOfYear}.csv", invalidTransactions);
                Console.WriteLine("Des transactions invalides ont été détectées et enregistrées dans invalid_transactions.csv");
            }
            else
            {
                Console.WriteLine("Toutes les transactions sont valides.");
            }
            return operations;
        }

        static async Task AjoutTauxChange(List<Operation> operations)
        {
            foreach (var operation in operations)
            {
                if (operation.Devise.ToString() != "EUR")
                {
                    operation.TauxDeChange = await GetTauxChange(operation.Devise.ToString());
                }
            }
            
        }

        public static async Task<double> GetTauxChange(string currency)
        {
            // Simulation d'un taux de change (à remplacer par une API externe si nécessaire)
            var rates = new Dictionary<string, double> { { "USD", 1.1 }, { "GBP", 0.85 }, { "JPY", 160.80} };
            return rates.ContainsKey(currency) ? rates[currency] : 1.0;
        }

        static void GenerateDailyJson(List<Operation> operations)
        {
            string json = JsonSerializer.Serialize(operations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\OperationGenerator\\Transactions\\transactions.json", json);
        }
    }
}