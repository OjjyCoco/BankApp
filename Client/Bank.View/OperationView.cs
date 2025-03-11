using System;
using System.Collections.Generic;
using Bank.Datas.Entities;

namespace Bank.Views
{
    public class OperationView
    {
        public void AfficherOperations(List<Operation> operations)
        {
            Console.WriteLine("\nListe des opérations :");
            foreach (var op in operations)
            {
                Console.WriteLine($"ID: {op.Id} | Carte: {op.NumCarte} | Type: {op.Type} | Montant: {op.Montant} {op.Devise} | Date: {op.Date}");
            }
        }

        public void AfficherOperationDetails(Operation operation)
        {
            if (operation == null)
            {
                Console.WriteLine("Opération introuvable !");
                return;
            }

            Console.WriteLine("\nDétails de l'opération :");
            Console.WriteLine($"ID: {operation.Id}");
            Console.WriteLine($"Carte: {operation.NumCarte}");
            Console.WriteLine($"Type: {operation.Type}");
            Console.WriteLine($"Montant: {operation.Montant} {operation.Devise}");
            Console.WriteLine($"Date: {operation.Date}");
        }
    }
}
