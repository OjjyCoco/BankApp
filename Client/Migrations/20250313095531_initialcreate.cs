using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bank.Datas.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodePostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientsPart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sexe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientsPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientsPart_Clients_Id",
                        column: x => x.Id,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientsPro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Siret = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    StatutJuridique = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdresseSiege = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplementSiege = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodePostalSiege = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VilleSiege = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientsPro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientsPro_Clients_Id",
                        column: x => x.Id,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompteBancaires",
                columns: table => new
                {
                    NumCompte = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOuverture = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Solde = table.Column<double>(type: "float", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompteBancaires", x => x.NumCompte);
                    table.ForeignKey(
                        name: "FK_CompteBancaires_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilisateurs_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarteBancaires",
                columns: table => new
                {
                    NumCarte = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateExpiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumCompte = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarteBancaires", x => x.NumCarte);
                    table.ForeignKey(
                        name: "FK_CarteBancaires_CompteBancaires_NumCompte",
                        column: x => x.NumCompte,
                        principalTable: "CompteBancaires",
                        principalColumn: "NumCompte",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumCarte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Montant = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumCompte = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_CompteBancaires_NumCompte",
                        column: x => x.NumCompte,
                        principalTable: "CompteBancaires",
                        principalColumn: "NumCompte",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Adresse", "CodePostal", "Complement", "Mail", "Nom", "Ville" },
                values: new object[,]
                {
                    { 1, "12, rue des Oliviers", "94000", "", "bety@gmail.com", "BETY", "CRETEIL" },
                    { 2, "125, rue LaFayette", "94120", "Digicode 1432", "info@axa.fr", "AXA", "FONTENAY SOUS BOIS" },
                    { 3, "10, rue des Olivies", "94300", "Etage 2", "bodin@gmail.com", "BODIN", "VIENCENNES" },
                    { 4, "36, quai des Orfèvres", "93500", "", "info@paul.fr", "PAUL", "ROISSY FRANCE" },
                    { 5, "15, rue de la République", "94120", "", "berris@gmail.com", "BERRIS", "FONTENAY SOUS BOIS" },
                    { 6, "32, rue E.Renan", "75002", "Bat. C", "contact@primark.fr", "PRIMARK", "PARIS" },
                    { 7, "25, rue de la Paix", "92100", "", "abenir@gmail.com", "ABENIR", "LA DEFENSE" },
                    { 8, "23, av P.Valery", "92100", "", "info@zara.fr", "ZARA", "LA DEFENSE" },
                    { 9, "3, avenue des Parcs", "93500", "", "bensaid@gmail.com", "BENSAID", "ROISSY EN France" },
                    { 10, "15, Place de la Bastille", "75003", "Fond de Cour", "contact@leonidas.fr", "LEONIDAS", "PARIS" },
                    { 11, "3, rue Lecourbe", "93200", "", "ababou@gmail.com", "ABABOU", "BAGNOLET" }
                });

            migrationBuilder.InsertData(
                table: "ClientsPart",
                columns: new[] { "Id", "DateNaissance", "Prenom", "Sexe" },
                values: new object[,]
                {
                    { 1, new DateTime(1985, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daniel", "M" },
                    { 3, new DateTime(1965, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Justin", "M" },
                    { 5, new DateTime(1977, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Karine", "F" },
                    { 7, new DateTime(1977, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alexandra", "F" },
                    { 9, new DateTime(1976, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georgia", "F" },
                    { 11, new DateTime(1970, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teddy", "M" }
                });

            migrationBuilder.InsertData(
                table: "ClientsPro",
                columns: new[] { "Id", "AdresseSiege", "CodePostalSiege", "ComplementSiege", "Siret", "StatutJuridique", "VilleSiege" },
                values: new object[,]
                {
                    { 2, "125, rue LaFayette", "94120", "Digicode 1432", "12548795641122", "SARL", "FONTENAY SOUS BOIS" },
                    { 4, "10, esplanade de la défense", "92060", "", "87459564455444", "EURL", "LA DEFENSE" },
                    { 6, "32, rue E.Renan", "75002", "Bat. C", "08755897458455", "SARL", "PARIS" },
                    { 8, "24, esplanade de la Défense", "92060", "Tour Franklin", "65895874587854", "SA", "LA DEFENSE" },
                    { 10, "10, rue de la Paix", "75008", "Tour Franklin", "91235987456832", "SAS", "PARIS" }
                });

            migrationBuilder.InsertData(
                table: "CompteBancaires",
                columns: new[] { "NumCompte", "ClientId", "DateOuverture", "Solde" },
                values: new object[,]
                {
                    { "151DZ247Z", 1, new DateTime(2005, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 25680.5 },
                    { "354SE553A", 2, new DateTime(1989, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 725621684.60000002 }
                });

            migrationBuilder.InsertData(
                table: "Utilisateurs",
                columns: new[] { "Id", "ClientId", "Login", "Password" },
                values: new object[] { 1, 2, "Axa", "test" });

            migrationBuilder.InsertData(
                table: "CarteBancaires",
                columns: new[] { "NumCarte", "DateExpiration", "NumCompte" },
                values: new object[,]
                {
                    { "4974018502231000", new DateTime(2028, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "354SE553A" },
                    { "4974018502231018", new DateTime(2030, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "151DZ247Z" },
                    { "4974018502231034", new DateTime(2027, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "354SE553A" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarteBancaires_NumCarte",
                table: "CarteBancaires",
                column: "NumCarte",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarteBancaires_NumCompte",
                table: "CarteBancaires",
                column: "NumCompte");

            migrationBuilder.CreateIndex(
                name: "IX_CompteBancaires_ClientId",
                table: "CompteBancaires",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CompteBancaires_NumCompte",
                table: "CompteBancaires",
                column: "NumCompte",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Id",
                table: "Operations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_NumCompte",
                table: "Operations",
                column: "NumCompte");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_ClientId",
                table: "Utilisateurs",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarteBancaires");

            migrationBuilder.DropTable(
                name: "ClientsPart");

            migrationBuilder.DropTable(
                name: "ClientsPro");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "CompteBancaires");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
