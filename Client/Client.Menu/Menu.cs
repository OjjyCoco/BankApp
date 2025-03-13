//using System;
//using System.Threading.Tasks;
//using Bank.Controllers;
//using System.Threading.Tasks;


//namespace Client.Menu
//{
//    class Menu
//    {
//        private static async Task Main(string[] args)
//        {
//            var operationController = new OperationController();
//            var clientController = new ClientController();

//            bool exit = false;

//            while (!exit)
//            {
//                Console.Clear();
//                Console.WriteLine("===== MENU PRINCIPAL =====");
//                Console.WriteLine("1 - Consulter les opérations");
//                Console.WriteLine("2 - Consulter les clients");
//                Console.WriteLine("0 - Quitter");
//                Console.Write("Choisissez une option : ");

//                string? choix = Console.ReadLine();

//                switch (choix)
//                {
//                    case "1":
//                        await operationController.AfficherToutesLesOperations();
//                        break;
//                    case "2":
//                        await clientController.AfficherTousLesClients();
//                        break;
//                    case "0":
//                        exit = true;
//                        Console.WriteLine("Fermeture de l'application...");
//                        break;
//                    default:
//                        Console.WriteLine("Option invalide, veuillez réessayer.");
//                        break;
//                }

//                if (!exit)
//                {
//                    Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
//                    Console.ReadKey();
//                }
//            }
//        }
//    }
//}
