using DailyOperationChecker.APIs;
using DailyOperationChecker.Entities;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DailyOperationChecker
{
    public class OperationChecker
    {
        static async Task Main()
        {
            DateTime date;
            date = DateTime.Now;
            List<Operation> operations = await VerifyTransactionsFromCsv($"C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\OperationGenerator\\Transactions\\operations-{date.DayOfYear}.csv");
            await AjoutTauxChange(operations);
            using (var context = new OperationDbContext())
            {
                foreach (var op in operations)
                {
                    Console.WriteLine($"CardId: {op.NumCarte}, TauxChange: {op.TauxDeChange}");
                    context.Operations.Add(op); // Ajout à la base
                    context.SaveChanges(); // Enregistrer dans la DB
                }

            }

            GenerateDailyJson(operations);
        }

        public static async Task<List<Operation>> VerifyTransactionsFromCsv(string filePath)
        {
            DateTime date;
            date = DateTime.Now;
            var invalidTransactions = new List<string>();
            List<string> codes = await ExchangeRateAPI.ObtenirCodesDevises();
            List<Operation> operations = new List<Operation> { };

            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine(); // Lire l'en-tête
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    string cardNumber = values[0];
                    try
                    {
                        OperationType oType = GetTypeToOperationType(values[2]);
                    }
                    catch
                    {
                        invalidTransactions.Add(line);
                        continue;
                    }
                    if (values.Length < 5 || !IsValidLuhn(cardNumber) || !codes.Contains(values[4]))
                    {
                        invalidTransactions.Add(line);
                    }
                    else
                    {
                        operations.Add(new Operation
                        {
                            NumCarte = values[0],
                            Montant = double.Parse(values[1]),
                            Type = values[2],
                            Date = DateTime.Parse(values[3]),
                            Devise = values[4]
                        });
                    }
                }
                var doublons = operations
                    .GroupBy(o => new { o.NumCarte, o.Date })
                    .Where(w => w.Count() > 1)
                    .SelectMany(w => w)
                    .ToList();

                if (doublons.Any())
                    foreach (var d in doublons)
                    {
                        invalidTransactions.Add($"{d.NumCarte},{d.Montant},{d.Type},{d.Date},{d.Devise}");
                        operations.Remove(d);
                    }


            }

            if (invalidTransactions.Count > 0)
            {
                File.WriteAllLines($"C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\OperationGenerator\\Transactions\\invalid_operations-{date.DayOfYear}.csv", invalidTransactions);
                Console.WriteLine("Des transactions invalides ont été détectées et enregistrées dans invalid_operations.csv");
            }
            else
            {
                Console.WriteLine("Toutes les transactions sont valides.");
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
            return sum % 10 == 0;
        }
        static async Task AjoutTauxChange(List<Operation> operations)
        {
            if (operations.Count == 0)
                return;

            List<string> devises = new List<string>();

            foreach (var operation in operations)
                devises.Add(operation.Devise);

            Dictionary<string, decimal> tauxDevises = await ExchangeRateAPI.ObtenirTauxChange(devises, operations[0].Date);

            foreach (var item in tauxDevises)
                operations.First(o => o.Devise == item.Key).TauxDeChange = item.Value;
        }




        static void GenerateDailyJson(List<Operation> operations)
        {
            DateTime date;
            date = DateTime.Now;
            List<OperationJson> opjList = new List<OperationJson>();
            foreach (var op in operations)
            {
                opjList.Add(new OperationJson
                {
                    NumCarte = op.NumCarte,
                    Date = op.Date,
                    Montant = op.Montant / (double)op.TauxDeChange,
                    Type = op.Type
                });

            }
            string json = JsonSerializer.Serialize(opjList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText($"C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\OperationGenerator\\Transactions\\operations-{date.DayOfYear}.json", json);
        }

        static OperationType GetTypeToOperationType(string type)
        {
            switch (type)
            {
                case "RetraitDAB":
                    return OperationType.RetraitDAB;
                case "FactureCB":
                    return OperationType.FactureCB;
                case "DepotGuichet":
                    return OperationType.DepotGuichet;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
