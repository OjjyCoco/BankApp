using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Views;
using Bank.Datas.Repositories;
using Bank.Datas.Entities;

namespace Bank.Controllers
{
    public class CompteBancaireController
    {
        private readonly CompteBancaireRepository _repo;
        private readonly CompteBancaireView _view;

        public CompteBancaireController()
        {
            _repo = new CompteBancaireRepository();
            _view = new CompteBancaireView();
        }

        public async Task AfficherTousLesComptes()
        {
            var comptes = await _repo.GetAll();
            _view.AfficherComptes(comptes);
        }

        public async Task AfficherDetailsNumCompte(string numCompte)
        {
            var compte = await _repo.GetByNumCompte(numCompte);
            _view.AfficherDetailsCompte(compte);
        }

        public async Task<List<CompteBancaire>> GetComptesByClient(int clientId)
        {
            var comptes = await _repo.GetComptesByClient(clientId);
            return comptes;
            //_view.AfficherComptes(comptes);
        }
    }
}
