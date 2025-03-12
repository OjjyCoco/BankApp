using System;
using System.Collections.Generic;
using Bank.Datas.Entities;

namespace Bank.Views
{
    public class OperationView
    {
        public void AfficherOperations(List<Operation> operations)
        {
            Console.WriteLine("\n╔══════════╦════════════╦═════════════╦═══════════════╦════════════════╗");
            Console.WriteLine("║   ID     ║   Carte    ║    Type     ║    Montant    ║      Date      ║");
            Console.WriteLine("╠══════════╬════════════╬═════════════╬═══════════════╬════════════════╣");

            foreach (var op in operations)
            {
                string numCarteMasque = MasquerNumCarte(op.NumCarte);
                string montantFormate = op.Montant.ToString("0.00"); // Tronque à 2 décimales

                Console.WriteLine($"║ {op.Id,-8} ║ {numCarteMasque,-10} ║ {op.Type,-11} ║ {montantFormate,-13}e║ {op.Date:dd/MM/yyyy} ║");
            }

            Console.WriteLine("╚══════════╩════════════╩═════════════╩═══════════════╩════════════════╝");
        }

        private string MasquerNumCarte(string numCarte)
        {
            return numCarte.Length == 16 ? "********" + numCarte[^4..] : "Numéro invalide";
        }


    }
}
