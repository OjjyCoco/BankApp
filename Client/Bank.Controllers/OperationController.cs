using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Bank.Datas.Entities;
using Bank.Views;
using Bank.Datas.Repositories;


namespace Bank.Controllers
{
    public class OperationController
    {
        private readonly OperationRepository _operationRepo;
        private readonly OperationView _operationView;
        private readonly CarteBancaireRepository _carteRepo;

        public OperationController()
        {
            _operationRepo = new OperationRepository();
            _operationView = new OperationView();
            _carteRepo = new CarteBancaireRepository();
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

        public async Task AfficherOperationsParCompte(string numCompte)
        {
            var operations = await _operationRepo.GetByNumCompte(numCompte);
            if (operations.Count == 0)
            {
                Console.WriteLine("Aucune opération trouvée pour ce compte !");
                return;
            }
            _operationView.AfficherOperations(operations);
        }

        public async Task AfficherOperationParId(int id)
        {
            var operation = await _operationRepo.GetById(id);
            _operationView.AfficherOperationDetails(operation);
        }

        public async Task AjouterOperation(Operation op)
        {
            bool success = await _operationRepo.Ajouter(op);
            Console.WriteLine(success ? "Opération ajoutée avec succès !" : "Échec de l'ajout de l'opération.");
        }

        public async Task ImporterOperationsDepuisJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Fichier JSON introuvable !");
                return;
            }

            try
            {
                string jsonContent = await File.ReadAllTextAsync(filePath);
                var operationsJson = JsonSerializer.Deserialize<List<Operation>>(jsonContent);

                if (operationsJson != null)
                {
                    foreach (var jsonOp in operationsJson)
                    {
                        var carte = await _carteRepo.GetById(jsonOp.NumCarte);
                        if (carte == null)
                        {
                            Console.WriteLine($"Carte {jsonOp.NumCarte} introuvable, opération ignorée.");
                            continue;
                        }

                        var operation = new Operation
                        {
                            NumCarte = jsonOp.NumCarte,
                            Montant = jsonOp.Montant,
                            Type = jsonOp.Type,
                            Date = jsonOp.Date,
                            NumCompte = carte.NumCompte
                        };

                        await _operationRepo.Ajouter(operation);
                    }
                    Console.WriteLine("Importation réussie des opérations depuis le fichier JSON.");
                }
                else
                {
                    Console.WriteLine("Aucune opération valide trouvée dans le fichier JSON.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'importation du fichier JSON : {ex.Message}");
            }
        }


        public async Task ExporterOperationsPdf(string filePath)
        {
            var operations = await _operationRepo.GetAll();
            if (operations.Count == 0)
            {
                Console.WriteLine("Aucune opération à exporter !");
                return;
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, fs);
                document.Open();
                document.Add(new Paragraph("Liste des opérations bancaires"));

                foreach (var op in operations)
                {
                    document.Add(new Paragraph($"ID: {op.Id}, Montant: {op.Montant}, Date: {op.Date}, Type: {op.Type}"));
                }
                
                document.Close();
            }

            Console.WriteLine("Fichier PDF généré avec succès.");
        }

        public async Task ExporterOperationsXml(string filePath)
        {
            var operations = await _operationRepo.GetAll();
            if (operations.Count == 0)
            {
                Console.WriteLine("Aucune opération à exporter !");
                return;
            }

            var xmlSerializer = new XmlSerializer(typeof(List<OperationExport>));
            var operationsExport = new List<OperationExport>();

            foreach (var op in operations)
            {
                operationsExport.Add(new OperationExport
                {
                    Id = op.Id,
                    Montant = op.Montant,
                    Date = op.Date,
                    Type = op.Type,
                    NumCompte = "XXXX-XXXX-XXXX-" + op.NumCompte.Substring(op.NumCompte.Length - 4) // Masquer une partie du numéro de compte
                });
            }

            using (TextWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                xmlSerializer.Serialize(writer, operationsExport);
            }

            Console.WriteLine("Fichier XML généré avec succès.");
        }

        public class OperationExport
        {
            public int Id { get; set; }
            public double Montant { get; set; }
            public DateTime Date { get; set; }
            public string Type { get; set; }
            public string NumCompte { get; set; } // Masqué
        }

        public class JsonOperation
        {
            public string NumCarte { get; set; }
            public double Montant { get; set; }
            public string Type { get; set; }
            public DateTime Date { get; set; }
        }



    }
}
