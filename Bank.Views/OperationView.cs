using System;
using System.Collections.Generic;
using Bank.Datas.Entities;

namespace Bank.Views
{
    public class OperationView
    {
        public void AfficherOperations(List<Operation> operations)
        {
            Console.WriteLine("\n===== Liste des opérations =====\n");

            foreach (var op in operations)
            {
                string numCarteMasque = MasquerNumCarte(op.NumCarte);
                string montantFormate = op.Montant.ToString("0.00"); // Format montant avec 2 décimales

                Console.WriteLine($"ID: {op.Id}");
                Console.WriteLine($"Carte: {numCarteMasque}");
                Console.WriteLine($"Type: {op.Type}");
                Console.WriteLine($"Montant: {montantFormate} euros");
                Console.WriteLine($"Date: {op.Date:dd/MM/yyyy}");
                Console.WriteLine(new string('-', 30)); // Ligne de séparation
            }
        }

        private string MasquerNumCarte(string numCarte)
        {
            return (numCarte?.Length == 16) ? "********" + numCarte[^4..] : "XXXXXXXXXX";
        }
    }
}
