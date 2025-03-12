using Bank.Datas.Entities;
using Microsoft.EntityFrameworkCore;
using Bank.Datas.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Datas.Repositories
{
    public class ClientRepository
    {
        public ClientRepository()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var context = new BankDbContext();
            context.Database.EnsureCreated();
        }

        public async Task<List<Client>> GetAll()
        {
            using var context = new BankDbContext();
            return await context.Clients.ToListAsync();
        }

        public async Task<Client?> GetById(int id)
        {
            using var context = new BankDbContext();
            return await context.Clients.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> Add(Client client)
        {
            using var context = new BankDbContext();
            context.Clients.Add(client);
            return await context.SaveChangesAsync();
        }

        public async Task<bool> Remove(int id)
        {
            using var context = new BankDbContext();
            var client = await context.Clients.FindAsync(id);
            if (client == null) return false;

            context.Clients.Remove(client);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
