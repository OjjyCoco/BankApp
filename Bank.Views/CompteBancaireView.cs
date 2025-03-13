using Bank.Datas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Views
{
    public class CompteBancaireView
    {
        public void AfficherComptes(List<CompteBancaire> comptes)
        {
            Console.WriteLine("Liste des comptes bancaires :");
            foreach (var compte in comptes)
            {
                Console.WriteLine($"Numéro: {compte.NumCompte} | Solde: {compte.Solde}€ | Date d'ouverture: {compte.DateOuverture.ToShortDateString()}");
            }
        }

        public void AfficherDetailsCompte(CompteBancaire? compte)
        {
            if (compte == null)
            {
                Console.WriteLine("Compte introuvable.");
                return;
            }
            Console.WriteLine($"Numéro: {compte.NumCompte}");
            Console.WriteLine($"Solde: {compte.Solde} euros");
            Console.WriteLine($"Date d'ouverture: {compte.DateOuverture.ToShortDateString()}");
        }
    }
}
