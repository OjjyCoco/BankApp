using Bank.Datas.Entities;
using Bank.Datas.Repositories;
using Bank.Datas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Datas.Repositories
{
    public class OperationRepository : IRepository<Operation>
    {
        public OperationRepository()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var context = new BankDbContext();
            context.Database.EnsureCreated();
        }

        public async Task<List<Operation>> GetAll()
        {
            using var context = new BankDbContext();
            var Operations = await context.Operations
                                        .ToListAsync<Operation>();
            return Operations;
        }

        public async Task<Operation?> GetById(int id)
        {
            using var context = new BankDbContext();
            return await context.Operations
                                .FirstOrDefaultAsync(op => op.Id == id);
        }

        public async Task<bool> Ajouter(Operation op)
        {
            using var context = new BankDbContext();
            try
            {
                await context.Operations.AddAsync(op);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
