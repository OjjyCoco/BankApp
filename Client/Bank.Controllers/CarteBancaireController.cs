using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Views;
using Bank.Datas.Repositories;
using Bank.Datas.Entities;

namespace Bank.Controllers
{
    public class CarteBancaireController
    {
        private readonly CarteBancaireRepository _repo;
        private readonly CarteBancaireView _view;

        public CarteBancaireController(CarteBancaireRepository repo, CarteBancaireView view)
        {
            _repo = repo;
            _view = view;
        }

        public async Task AfficherToutesLesCartes()
        {
            var cartes = await _repo.GetAll();
            _view.AfficherCartes(cartes);
        }

        public async Task AjouterCarte(CarteBancaire carte)
        {
            await _repo.Add(carte);
            Console.WriteLine($"Carte {carte.NumCarte} ajoutée !");
        }

        public async Task SupprimerCarte(int numCarte)
        {
            var result = await _repo.Remove(numCarte);
            if (result)
                Console.WriteLine($"Carte {numCarte} supprimée !");
            else
                Console.WriteLine($"Carte {numCarte} introuvable !");
        }
    }
}
