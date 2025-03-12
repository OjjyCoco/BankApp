using Microsoft.EntityFrameworkCore;
using Bank.Datas.Entities;
using System.Threading.Tasks;
using Bank.Datas;

namespace Recap.Datas.Repositories
{
    public class UtilisateurRepository
    {
        public async Task<Utilisateur?> GetByLogin(string login)
        {
            using var context = new BankDbContext();
            return await context.Utilisateurs
                                .Where(u => u.Login == login)
                                .SingleOrDefaultAsync();
        }
    }
}
