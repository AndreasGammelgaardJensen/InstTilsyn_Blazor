using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VuggestueTilsynScraper.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Vej = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip_code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pladser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VuggestuePladser = table.Column<int>(type: "int", nullable: false),
                    BoernehavePladser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pladser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionFrontPageModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeEnum = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    addressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pladserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    homePage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    profile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionFrontPageModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionFrontPageModel_Address_addressId",
                        column: x => x.addressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionFrontPageModel_Pladser_pladserId",
                        column: x => x.pladserId,
                        principalTable: "Pladser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionFrontPageModel_addressId",
                table: "InstitutionFrontPageModel",
                column: "addressId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionFrontPageModel_pladserId",
                table: "InstitutionFrontPageModel",
                column: "pladserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionFrontPageModel");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Pladser");
        }
    }
}
