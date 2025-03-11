using Bank.Datas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Views
{
    public class CarteBancaireView
    {
        public void AfficherCartes(List<CarteBancaire> cartes)
        {
            Console.WriteLine("Liste des cartes bancaires :");
            foreach (var carte in cartes)
            {
                Console.WriteLine($"Numéro: {carte.NumCarte} | Expiration: {carte.DateExpiration.ToShortDateString()}");
            }
        }
    }
}
