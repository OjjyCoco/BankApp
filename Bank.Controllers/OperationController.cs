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
using System.Globalization;

namespace Bank.Controllers
{
    public class OperationController
    {
        private readonly OperationRepository _operationRepo;
        private readonly OperationView _operationView;
        private readonly CarteBancaireRepository _carteRepo;
        private readonly CompteBancaireRepository _compteRepo;

        public OperationController()
        {
            _operationRepo = new OperationRepository();
            _operationView = new OperationView();
            _carteRepo = new CarteBancaireRepository();
            _compteRepo = new CompteBancaireRepository();
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

                        // on créer une nouvelle opération
                        var operation = new Operation
                        {
                            NumCarte = jsonOp.NumCarte,
                            Montant = jsonOp.Montant,
                            Type = jsonOp.Type,
                            Date = jsonOp.Date,
                            NumCompte = carte.NumCompte
                        };

                        // on l'ajoute à la base de données
                        bool ajoutOk = await _operationRepo.Ajouter(operation);
                        if (ajoutOk)
                        {
                            await MettreAJourSoldeCompte(operation);
                        }
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

        // Méthode pour mettre à jour le solde du compte après qu'une opération a été ajoutée dans ImporterOperationsDepuisJson
        private async Task MettreAJourSoldeCompte(Operation operation)
        {
            var compte = await _compteRepo.GetByNumCompte(operation.NumCompte);
            double ancienSolde = compte.Solde;
            if (compte == null)
            {
                Console.WriteLine($"Compte {operation.NumCompte} introuvable, impossible de mettre à jour le solde.");
                return;
            }

            if (operation.Type == "FactureCB" || operation.Type == "RetraitDAB")
            {
                if (compte.Solde >= operation.Montant)
                {
                    compte.Solde -= operation.Montant;
                }
                else
                {
                    Console.WriteLine("-- Opération rejetée --");
                    Console.WriteLine($"Solde insuffisant pour effectuer {operation.Type} sur compte numéro {compte.NumCompte}.\nOpération annulée, base de donnée pas mise à jour. \n");
                    return;
                }
            }
            else if (operation.Type == "DepotGuichet")
            {
                compte.Solde += operation.Montant;
            }
            else
            {
                Console.WriteLine($"Type d'opération non reconnu : {operation.Type}");
                return;
            }

            bool success = await _compteRepo.MettreAJourSolde(compte);
            Console.WriteLine("-- Nouvelle opération --");
            Console.WriteLine(success ? $"Solde du compte numéro {compte.NumCompte} mis à jour après {operation.Type} d'un montant de {operation.Montant:0.00} euro(s)" +
                $" \n Ancien solde : {ancienSolde}  \n Nouveau solde : {compte.Solde:0.00} euros \n" : "Erreur lors de la mise à jour du solde. \n");

        }


        public async Task ExporterOperationsPdf(string NumCompte, int mois, int annee)
        {
            CultureInfo cultureFr = new CultureInfo("fr-FR");
            string moisNom = cultureFr.DateTimeFormat.GetMonthName(mois);

            var operations = await _operationRepo.GetByNumCompte(NumCompte, mois, annee);
            if (operations.Count == 0)
            {
                Console.WriteLine("Aucune opération à exporter pour cette période !");
                return;
            }

            using (FileStream fs = new FileStream($"C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\operations-{moisNom}-{annee}.pdf", FileMode.Create))
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(document, fs);
                document.Open();

                // Ajout d'un titre
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph title = new Paragraph($"Relevé des opérations - Compte {NumCompte} - Mois de {moisNom} {annee}", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20;
                document.Add(title);

                // Création du tableau avec colonnes
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 15f, 30f, 25f, 30f }); // Largeur relative des colonnes

                // Définition des styles d'en-tête
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                PdfPCell cell;

                // Ajout en-têtes
                string[] headers = { "ID", "Numéro Carte", "Montant (€)", "Date et Type" };
                foreach (string header in headers)
                {
                    cell = new PdfPCell(new Phrase(header, headerFont));
                    cell.BackgroundColor = new BaseColor(230, 230, 230); // Gris clair
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    table.AddCell(cell);
                }

                // Style pour le contenu
                Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // Ajout des oprations
                foreach (var op in operations)
                {
                    string numCarteMasque = MasquerNumCarte(op.NumCarte);

                    table.AddCell(new PdfPCell(new Phrase(op.Id.ToString(), contentFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(numCarteMasque, contentFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase($"{op.Montant:F2} €", contentFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase($"{op.Date:dd/MM/yyyy} - {op.Type}", contentFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                }

                document.Add(table);
                document.Close();
            }

            Console.WriteLine("Fichier PDF généré avec succès.");
        }

        // Méthode pour masquer les 4 derniers chiffres du num de catre
        private string MasquerNumCarte(string numCarte)
        {
            if (numCarte.Length == 16)
            {
                return "**** **** **** " + numCarte.Substring(12, 4);
            }
            return "Numéro invalide";
        }

    }
}
