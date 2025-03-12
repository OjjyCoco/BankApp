using Bank.Datas;
using Bank.Datas.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Datas.Repositories
{
    public class CarteBancaireRepository
    {
        public CarteBancaireRepository()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var context = new BankDbContext();
            context.Database.EnsureCreated();
        }

        public async Task<List<CarteBancaire>> GetAll()
        {
            using var context = new BankDbContext();
            return await context.CarteBancaires.ToListAsync();
        }

        public async Task<CarteBancaire?> GetById(string numCarte)
        {
            using var context = new BankDbContext();
            return await context.CarteBancaires.SingleOrDefaultAsync(c => c.NumCarte == numCarte);
        }

        public async Task<int> Add(CarteBancaire carte)
        {
            using var context = new BankDbContext();
            context.CarteBancaires.Add(carte);
            return await context.SaveChangesAsync();
        }

        public async Task<bool> Remove(int numCarte)
        {
            using var context = new BankDbContext();
            var carte = await context.CarteBancaires.FindAsync(numCarte);
            if (carte == null) return false;

            context.CarteBancaires.Remove(carte);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
