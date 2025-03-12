using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationGenerator.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumCarte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Montant = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Devise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TauxDeChange = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "Date", "Devise", "Montant", "NumCarte", "TauxDeChange", "Type" },
                values: new object[] { 1, new DateTime(2024, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "JPY", 4588.1499999999996, "6884787530561463", 110m, "RetraitDAB" });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Id",
                table: "Operations",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations");
        }
    }
}
