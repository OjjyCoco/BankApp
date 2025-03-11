using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Datas.Entities;
using Bank.Views;
using Bank.Datas.Repositories;
using System.Threading.Tasks;

namespace Bank.Controllers
{
    public class OperationController
    {
        private readonly OperationRepository _operationRepo;
        private readonly OperationView _operationView;

        public OperationController()
        {
            _operationRepo = new OperationRepository();
            _operationView = new OperationView();
        }

        public async Task AfficherToutesLesOperations()
        {
            var operations = await _operationRepo.GetAll();
            if (operations.Count == 0)
            {
                Console.WriteLine("Aucune opération trouvée !");
                return;
            }

            _operationView.AfficherOperations(operations);
        }

        public async Task AfficherOperation(int id)
        {
            var operation = await _operationRepo.GetById(id);
            _operationView.AfficherOperationDetails(operation);
        }

        public async Task AjouterOperation(Operation op)
        {
            bool success = await _operationRepo.Ajouter(op);
            Console.WriteLine(success ? "Opération ajoutée avec succès !" : "Échec de l'ajout de l'opération.");
        }
    }
}
