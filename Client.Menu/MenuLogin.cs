using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Bank.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.Menu
{
    class MenuLogin
    {
        private static async Task Main(string[] args)
        {

            // On importe les opérations si besoin
            await ImporterOperationsSiNecessaire();

            var authService = new AuthService();
            bool authentifie = false;
            int? clientId = null;

            while (!authentifie)
            {
                Console.Clear();
                Console.WriteLine("===== AUTHENTIFICATION =====");
                Console.Write("Login: ");
                string? login = Console.ReadLine();

                Console.Write("Mot de passe: ");
                string? password = LireMotDePasseCache();

                var (isAuthenticated, id) = await authService.Authentifier(login, password);
                authentifie = isAuthenticated;
                clientId = id;

                if (!authentifie)
                {
                    Console.WriteLine("\nÉchec de connexion. Réessayez.\n");
                    Console.ReadKey();
                }
            }

            // Affichage du menu principal pour un client authentifié
            await AfficherMenu(clientId.Value);
        }

        static async Task AfficherMenu(int clientId)
        {
            var operationController = new OperationController();
            var compteController = new CompteBancaireController();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("===== MENU PRINCIPAL =====");
                Console.WriteLine("1 - Consulter toutes mes opérations (par compte)");
                Console.WriteLine("2 - Exporter PDF de mes opérations (par compte)");
                Console.WriteLine("3 - Consulter mes informations de compte");
                Console.WriteLine("0 - Quitter");
                Console.Write("Choisissez une option : ");

                string? choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        string? numCompte = await ChoisirCompte(clientId);
                        if (!string.IsNullOrEmpty(numCompte))
                        {
                            await operationController.AfficherOperationsParCompte(numCompte);
                        }
                        else
                        {
                            Console.WriteLine("Aucun compte sélectionné.");
                        }
                        break;
                    case "2": // Exporter PDF
                        var periode = ChoisirMoisEtAnnee();
                        if (periode == null)
                        {
                            Console.WriteLine("Export annulé.");
                            break;
                        }

                        string? numCompte2 = await ChoisirCompte(clientId);
                        if (!string.IsNullOrEmpty(numCompte2))
                        {
                            await operationController.ExporterOperationsPdf(numCompte2, periode.Value.mois, periode.Value.annee);
                        }
                        else
                        {
                            Console.WriteLine("Aucun compte sélectionné.");
                        }
                        break;


                    case "3":
                        string? numCompte3 = await ChoisirCompte(clientId);
                        if (!string.IsNullOrEmpty(numCompte3))
                        {
                            await compteController.AfficherDetailsNumCompte(numCompte3);
                        }
                        else
                        {
                            Console.WriteLine("Aucun compte sélectionné.");
                        }
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Fermeture de l'application...");
                        break;
                    default:
                        Console.WriteLine("Option invalide, veuillez réessayer.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                    Console.ReadKey();
                }
            }
        }

        // Méthode pour choisir un compte parmi ceux du client quand il en a beosoin
        static async Task<string?> ChoisirCompte(int clientId)
        {
            var compteController = new CompteBancaireController();
            var comptes = await compteController.GetComptesByClient(clientId);

            if (comptes == null || comptes.Count == 0)
            {
                Console.WriteLine("Aucun compte trouvé pour ce client.");
                return null;
            }

            Console.WriteLine("Sélectionnez un compte :");
            for (int i = 0; i < comptes.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {comptes[i].NumCompte}");
            }

            Console.Write("Votre choix : ");
            if (int.TryParse(Console.ReadLine(), out int choix) && choix > 0 && choix <= comptes.Count)
            {
                return comptes[choix - 1].NumCompte;
            }

            Console.WriteLine("Choix invalide.");
            return null;
        }

        static string LireMotDePasseCache()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(intercept: true);
                if (key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }


        // Méthode pour importer les opérations depuis les fichiers JSON seulement si on les a pas déjà importées
        static async Task ImporterOperationsSiNecessaire()
        {
            var operationController = new OperationController();
            string lastFetchFile = "C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\lastfetchoperations.json";

            // Lire le dernier numéro du jour importé
            int dernierJourImporte = 0;
            if (File.Exists(lastFetchFile))
            {
                string lastFetchContent = await File.ReadAllTextAsync(lastFetchFile);
                if (int.TryParse(lastFetchContent, out int lastDay))
                {
                    dernierJourImporte = lastDay;
                }
            }

            // Jour actuel
            int jourActuel = DateTime.Now.DayOfYear;

            // Importer tous les fichiers depuis le dernier jour importé
            bool nouvelImport = false;
            for (int jour = dernierJourImporte + 1; jour <= jourActuel; jour++)
            {

                string fichierAImporter = $"C:\\Users\\yohan\\Documents\\POEIHN\\ProjetNET\\OperationGenerator\\Transactions\\operations-{jour}.json";
                if (File.Exists(fichierAImporter))
                {
                    await operationController.ImporterOperationsDepuisJson(fichierAImporter);
                  
                    // Mettre à jour le fichier de suivi après chaque import réussi
                    await File.WriteAllTextAsync(lastFetchFile, jour.ToString());
                    Console.WriteLine($"Opérations du jour {jour} importées avec succès.");
                    nouvelImport = true;
                }
            }

            if (!nouvelImport)
            {
                Console.WriteLine("Aucune nouvelle opération à importer.");
            }
            Console.ReadKey();
        }

        // Méthode qui permet à l'utilisateur de choisir son mois et son année
        // Pour l'instant uniquement utilisée pour l'export PDF
        static (int mois, int annee)? ChoisirMoisEtAnnee()
        {
            Console.Write("Entrez le mois (1-12) : ");
            if (!int.TryParse(Console.ReadLine(), out int mois) || mois < 1 || mois > 12)
            {
                Console.WriteLine("Mois invalide.");
                return null;
            }

            Console.Write("Entrez l'année souhaitée : ");
            if (!int.TryParse(Console.ReadLine(), out int annee) || annee < 1900 || annee > DateTime.Now.Year)
            {
                Console.WriteLine("Année invalide.");
                return null;
            }

            return (mois, annee);
        }

    }
}
