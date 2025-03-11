using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Views;
using Bank.Datas.Repositories;
using Bank.Datas.Entities;

namespace Recap.Controllers
{
    public class CompteBancaireController
    {
        private readonly CompteBancaireRepository _repo;
        private readonly CompteBancaireView _view;

        public CompteBancaireController(CompteBancaireRepository repo, CompteBancaireView view)
        {
            _repo = repo;
            _view = view;
        }

        public async Task AfficherTousLesComptes()
        {
            var comptes = await _repo.GetAll();
            _view.AfficherComptes(comptes);
        }

        public async Task AjouterCompte(CompteBancaire compte)
        {
            await _repo.Add(compte);
            Console.WriteLine($"Compte {compte.NumCompte} ajouté !");
        }

        public async Task SupprimerCompte(int numCompte)
        {
            var result = await _repo.Remove(numCompte);
            if (result)
                Console.WriteLine($"Compte {numCompte} supprimé !");
            else
                Console.WriteLine($"Compte {numCompte} introuvable !");
        }
    }
}
