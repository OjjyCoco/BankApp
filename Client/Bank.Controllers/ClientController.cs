using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Views;
using Bank.Datas.Repositories;
using Bank.Datas.Entities;

namespace Bank.Controllers
{
    public class ClientController
    {
        private readonly ClientRepository _repo;
        private readonly ClientView _view;

        public ClientController()
        {
            _repo = new ClientRepository();
            _view = new ClientView();
        }

        public async Task AfficherTousLesClients()
        {
            var clients = await _repo.GetAll();
            _view.AfficherClients(clients);
        }

        public async Task AjouterClient(Client client)
        {
            await _repo.Add(client);
            Console.WriteLine($"Client {client.Nom} ajouté !");
        }
    }
}
