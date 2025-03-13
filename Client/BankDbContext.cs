using Bank.Datas.Entities;
using Microsoft.EntityFrameworkCore;


namespace Bank.Datas
{
    public class BankDbContext : DbContext
    {

        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientPart> ClientsPart { get; set; }

        public DbSet<ClientPro> ClientsPro { get; set; }

        public DbSet<Utilisateur> Utilisateurs { get; set; }

        public DbSet<CompteBancaire> CompteBancaires { get; set; }

        public DbSet<CarteBancaire> CarteBancaires { get; set; }

        public DbSet<Operation> Operations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BankDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //TPT
            modelBuilder.Entity<ClientPro>().ToTable("ClientsPro");
            modelBuilder.Entity<ClientPart>().ToTable("ClientsPart");
            modelBuilder.Entity<Utilisateur>().ToTable("Utilisateurs");
            modelBuilder.Entity<CarteBancaire>().ToTable("CarteBancaires");
            modelBuilder.Entity<CompteBancaire>().ToTable("CompteBancaires");
            modelBuilder.Entity<Operation>().ToTable<Operation>("Operations");

            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Operation>()
                .HasIndex(o => o.Id)
                .IsUnique();


            modelBuilder.Entity<CompteBancaire>()
                .HasIndex(c => c.NumCompte)
                .IsUnique();

            modelBuilder.Entity<CarteBancaire>()
                .HasIndex(c => c.NumCarte)
                .IsUnique();

            //Stocker en base la chaine de caracteres
            modelBuilder.Entity<ClientPart>().Property(c => c.Sexe).HasConversion<string>();

            modelBuilder.Entity<ClientPro>().Property(c => c.StatutJuridique).HasConversion<string>();


            modelBuilder.Entity<ClientPro>().HasData(
                new ClientPro
                {
                    Id = 2,
                    Nom = "AXA",
                    Adresse = "125, rue LaFayette",
                    Complement = "Digicode 1432",
                    CodePostal = "94120",
                    Ville = "FONTENAY SOUS BOIS",
                    Mail = "info@axa.fr",
                    Siret = "12548795641122",
                    StatutJuridique = StatutJuridique.SARL,
                    AdresseSiege = "125, rue LaFayette",
                    ComplementSiege = "Digicode 1432",
                    CodePostalSiege = "94120",
                    VilleSiege = "FONTENAY SOUS BOIS"
                });

            modelBuilder.Entity<ClientPro>().HasData(
                new ClientPro
                {
                    Id = 4,
                    Nom = "PAUL",
                    Adresse = "36, quai des Orfèvres",
                    Complement = "",
                    CodePostal = "93500",
                    Ville = "ROISSY FRANCE",
                    Mail = "info@paul.fr",
                    Siret = "87459564455444",
                    StatutJuridique = StatutJuridique.EURL,
                    AdresseSiege = "10, esplanade de la défense",
                    ComplementSiege = "",
                    CodePostalSiege = "92060",
                    VilleSiege = "LA DEFENSE"
                });

            modelBuilder.Entity<ClientPro>().HasData(
                new ClientPro
                {
                    Id = 6,
                    Nom = "PRIMARK",
                    Adresse = "32, rue E.Renan",
                    Complement = "Bat. C",
                    CodePostal = "75002",
                    Ville = "PARIS",
                    Mail = "contact@primark.fr",
                    Siret = "08755897458455",
                    StatutJuridique = StatutJuridique.SARL,
                    AdresseSiege = "32, rue E.Renan",
                    ComplementSiege = "Bat. C",
                    CodePostalSiege = "75002",
                    VilleSiege = "PARIS"
                });

            modelBuilder.Entity<ClientPro>().HasData(
                new ClientPro
                {
                    Id = 8,
                    Nom = "ZARA",
                    Adresse = "23, av P.Valery",
                    Complement = "",
                    CodePostal = "92100",
                    Ville = "LA DEFENSE",
                    Mail = "info@zara.fr",
                    Siret = "65895874587854",
                    StatutJuridique = StatutJuridique.SA,
                    AdresseSiege = "24, esplanade de la Défense",
                    ComplementSiege = "Tour Franklin",
                    CodePostalSiege = "92060",
                    VilleSiege = "LA DEFENSE"
                });

            modelBuilder.Entity<ClientPro>().HasData(
                new ClientPro
                {
                    Id = 10,
                    Nom = "LEONIDAS",
                    Adresse = "15, Place de la Bastille",
                    Complement = "Fond de Cour",
                    CodePostal = "75003",
                    Ville = "PARIS",
                    Mail = "contact@leonidas.fr",
                    Siret = "91235987456832",
                    StatutJuridique = StatutJuridique.SAS,
                    AdresseSiege = "10, rue de la Paix",
                    ComplementSiege = "Tour Franklin",
                    CodePostalSiege = "75008",
                    VilleSiege = "PARIS"
                });

            modelBuilder.Entity<ClientPart>().HasData(
                new ClientPart
                {
                    Id = 1,
                    Nom = "BETY",
                    Adresse = "12, rue des Oliviers",
                    Complement = "",
                    Ville = "CRETEIL",
                    CodePostal = "94000",
                    Mail = "bety@gmail.com",
                    Prenom = "Daniel",
                    Sexe = Sexe.M,
                    DateNaissance = new DateTime(1985, 11, 12)
                });

            modelBuilder.Entity<ClientPart>().HasData(
                new ClientPart
                {
                    Id = 3,
                    Nom = "BODIN",
                    Adresse = "10, rue des Olivies",
                    Complement = "Etage 2",
                    Ville = "VIENCENNES",
                    CodePostal = "94300",
                    Mail = "bodin@gmail.com",
                    Prenom = "Justin",
                    Sexe = Sexe.M,
                    DateNaissance = new DateTime(1965, 05, 05)
                });

            modelBuilder.Entity<ClientPart>().HasData(
                new ClientPart
                {
                    Id = 5,
                    Nom = "BERRIS",
                    Adresse = "15, rue de la République",
                    Complement = "",
                    Ville = "FONTENAY SOUS BOIS",
                    CodePostal = "94120",
                    Mail = "berris@gmail.com",
                    Prenom = "Karine",
                    Sexe = Sexe.F,
                    DateNaissance = new DateTime(1977, 06, 06)
                });

            modelBuilder.Entity<ClientPart>().HasData(
                new ClientPart
                {
                    Id = 7,
                    Nom = "ABENIR",
                    Adresse = "25, rue de la Paix",
                    Complement = "",
                    Ville = "LA DEFENSE",
                    CodePostal = "92100",
                    Mail = "abenir@gmail.com",
                    Prenom = "Alexandra",
                    Sexe = Sexe.F,
                    DateNaissance = new DateTime(1977, 04, 12)
                });

            modelBuilder.Entity<ClientPart>().HasData(
                new ClientPart
                {
                    Id = 9,
                    Nom = "BENSAID",
                    Adresse = "3, avenue des Parcs",
                    Complement = "",
                    Ville = "ROISSY EN France",
                    CodePostal = "93500",
                    Mail = "bensaid@gmail.com",
                    Prenom = "Georgia",
                    Sexe = Sexe.F,
                    DateNaissance = new DateTime(1976, 04, 16)
                });

            modelBuilder.Entity<ClientPart>().HasData(
                new ClientPart
                {
                    Id = 11,
                    Nom = "ABABOU",
                    Adresse = "3, rue Lecourbe",
                    Complement = "",
                    Ville = "BAGNOLET",
                    CodePostal = "93200",
                    Mail = "ababou@gmail.com",
                    Prenom = "Teddy",
                    Sexe = Sexe.M,
                    DateNaissance = new DateTime(1970, 10, 10)
                });


            modelBuilder.Entity<Utilisateur>().HasData(
                new Utilisateur
                {
                    Id = 1,
                    Login = "Axa",
                    Password = "test",
                    ClientId = 2
                });

            modelBuilder.Entity<CompteBancaire>().HasData(
                new CompteBancaire
                {
                    NumCompte = "151DZ247Z",
                    DateOuverture = new DateTime(2005, 05, 10),                    
                    ClientId = 1
                });

            modelBuilder.Entity<CompteBancaire>().HasData(
                new CompteBancaire
                {
                    NumCompte = "354SE553A",
                    DateOuverture = new DateTime(1989, 04, 22),
                    ClientId = 2
                });

            modelBuilder.Entity<CompteBancaire>().HasData(
                new CompteBancaire
                {
                    NumCompte = "354SE553B",
                    DateOuverture = new DateTime(1989, 07, 22),
                    ClientId = 2
                });

            modelBuilder.Entity<CarteBancaire>().HasData(
                new CarteBancaire
                {
                    NumCarte = "4974018502231018",
                    DateExpiration = new DateTime(2030, 01, 04),
                    NumCompte = "151DZ247Z"
                });

            modelBuilder.Entity<CarteBancaire>().HasData(
                new CarteBancaire
                {
                    NumCarte = "4974018502231026",
                    DateExpiration = new DateTime(2030, 01, 04),
                    NumCompte = "354SE553B"
                });
            modelBuilder.Entity<CarteBancaire>().HasData(
                new CarteBancaire
                {
                    NumCarte = "4974018502231034",
                    DateExpiration = new DateTime(2027, 06, 13),
                    NumCompte = "354SE553A"
                });

            modelBuilder.Entity<CarteBancaire>().HasData(
                new CarteBancaire
                {
                    NumCarte = "4974018502231000",
                    DateExpiration = new DateTime(2028, 03, 24),
                    NumCompte = "354SE553A"
                });

        }
    }


}