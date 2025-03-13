using Recap.Datas.Repositories;
using System;
using System.Threading.Tasks;

namespace Client.Menu
{
    public class AuthService
    {
        private readonly UtilisateurRepository _userRepo;

        public AuthService()
        {
            _userRepo = new UtilisateurRepository();
        }

        public async Task<(bool, int?)> Authentifier(string login, string password)
        {
            var user = await _userRepo.GetByLogin(login);
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
