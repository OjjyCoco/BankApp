using DailyOperationChecker.Entities;
using DailyOperationChecker.APIs;

namespace OperationGen.Programmes
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
                    Type = GetOperationString((OperationType)random.Next(Enum.GetValues(typeof(OperationType)).Length)),
                    Date = DateTime.Now,
                    Devise = codes[random.Next(codes.Count) - 1]   
                });
                await Task.Delay(3000);
                i++;
            }
            return operations;
        }

        static string GetOperationString(OperationType operation)
        {
            switch (operation)
            {
                case OperationType.RetraitDAB:
                    return "RetraitDAB";
                case OperationType.FactureCB:
                    return "FactureCB";
                case OperationType.DepotGuichet:
                    return "DepotGuichet";
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        static void GenererCsv(List<Operation> operations)
        {
            DateTime date;
            date = DateTime.Now;
            using (var writer = new StreamWriter($"C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\OperationGenerator\\Transactions\\operations-{date.DayOfYear}.csv"))
            {
                
                writer.WriteLine("NumCarte,Montant,OperationType,Date,Devise");
                foreach (var operation in operations)
                {
                    writer.WriteLine($"{operation.NumCarte},{operation.Montant},{operation.Type},{operation.Date:yyyy-MM-dd HH:mm:ss},{operation.Devise}");
                }
            }
        }

        

        
    }
}