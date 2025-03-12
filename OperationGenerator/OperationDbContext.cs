using Microsoft.EntityFrameworkCore;
using OperationGenerator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationGenerator
{
    public class OperationDbContext : DbContext
    {
        public DbSet<Operation> Operations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OperationDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //TPT
            modelBuilder.Entity<Operation>().ToTable<Operation>("Operations");

            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Operation>()
                .HasIndex(o => o.Id)
                .IsUnique();




            //Stocker en base la chaine de caracteres
            

            modelBuilder.Entity<Operation>().HasData(
                new Operation
                {
                    Id = 1,
                    NumCarte = "6884787530561463",
                    Montant = 4588.15,
                    Type = "RetraitDAB",
                    Date = new DateTime(2024, 03, 24),
                    Devise = "JPY",
                    TauxDeChange = 110
                });

        }
    }
}
