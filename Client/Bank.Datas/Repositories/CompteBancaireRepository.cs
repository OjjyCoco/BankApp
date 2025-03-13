using Bank.Datas;
using Bank.Datas.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Datas.Repositories
{
    public class CompteBancaireRepository
    {
        public CompteBancaireRepository()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var context = new BankDbContext();
            context.Database.EnsureCreated();
        }

        public async Task<List<CompteBancaire>> GetAll()
        {
            using var context = new BankDbContext();
            return await context.CompteBancaires.ToListAsync();
        }

        public async Task<CompteBancaire?> GetByNumCompte(string numCompte)
        {
            using var context = new BankDbContext();
            return await context.CompteBancaires.SingleOrDefaultAsync(c => c.NumCompte == numCompte);
        }

        public async Task<int> Add(CompteBancaire compte)
        {
            using var context = new BankDbContext();
            context.CompteBancaires.Add(compte);
            return await context.SaveChangesAsync();
        }

        public async Task<bool> Remove(int numCompte)
        {
            using var context = new BankDbContext();
            var compte = await context.CompteBancaires.FindAsync(numCompte);
            if (compte == null) return false;

            context.CompteBancaires.Remove(compte);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CompteBancaire>> GetComptesByClient(int clientId)
        {
            using var context = new BankDbContext();
            return await context.CompteBancaires
                                .Where(c => c.ClientId == clientId)
                                .ToListAsync();
        }

        public async Task<bool> MettreAJourSolde(CompteBancaire compte)
        {
            using var context = new BankDbContext();
            var compteDb = await context.CompteBancaires.SingleOrDefaultAsync(c => c.NumCompte == compte.NumCompte);
            if (compteDb == null) return false;

            compteDb.Solde = compte.Solde;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
