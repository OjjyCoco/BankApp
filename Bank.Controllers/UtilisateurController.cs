using System.Threading.Tasks;
using Recap.Datas.Repositories;
using Bank.Datas.Entities;

namespace Bank.Controllers
{
    public class UtilisateurController
    {
        private readonly UtilisateurRepository _userRepo;

        public UtilisateurController()
        {
            _userRepo = new UtilisateurRepository();
        }

        public async Task<Utilisateur?> GetByLogin(string login)
        {
            return await _userRepo.GetByLogin(login);
        }

        public async Task<(bool, int?)> Authentifier(string login, string password)
        {
            var user = await GetByLogin(login);
            if (user == null)
            {
                Console.WriteLine("Utilisateur introuvable.");
                return (false, null);
            }

            // Comparer le mot de passe (hashé idéalement)
            if (user.Password != password)
            {
                Console.WriteLine("Mot de passe incorrect.");
                return (false, null);
            }

            Console.WriteLine($"Connexion réussie ! Bienvenue {user.Login} !");
            Console.ReadKey();
            return (true, user.ClientId);
        }
    }
}