using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VuggestueTilsynScraper.Migrations
{
    /// <inheritdoc />
    public partial class ReportsAndCoordinates_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_Address_addressId",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_InstKoordinates_KoordinatesId",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_Pladser_pladserId",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "InstitutionTilsynsRapport");

            migrationBuilder.DropTable(
                name: "InstKoordinates");

            migrationBuilder.DropTable(
                name: "Pladser");

            migrationBuilder.CreateTable(
                name: "AddressDatabasemodel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Vej = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip_code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressDatabasemodel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionTilsynsRapportDatabasemodel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    documentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstitutionFrontPageModelDatabasemodelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionTilsynsRapportDatabasemodel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionTilsynsRapportDatabasemodel_InstitutionFrontPageModel_InstitutionFrontPageModelDatabasemodelId",
                        column: x => x.InstitutionFrontPageModelDatabasemodelId,
                        principalTable: "InstitutionFrontPageModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InstKoordinatesDatabasemodel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    lng = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstKoordinatesDatabasemodel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PladserDatabasemodel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VuggestuePladser = table.Column<int>(type: "int", nullable: false),
                    BoernehavePladser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PladserDatabasemodel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionTilsynsRapportDatabasemodel_InstitutionFrontPageModelDatabasemodelId",
                table: "InstitutionTilsynsRapportDatabasemodel",
                column: "InstitutionFrontPageModelDatabasemodelId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_AddressDatabasemodel_addressId",
                table: "InstitutionFrontPageModel",
                column: "addressId",
                principalTable: "AddressDatabasemodel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_InstKoordinatesDatabasemodel_KoordinatesId",
                table: "InstitutionFrontPageModel",
                column: "KoordinatesId",
                principalTable: "InstKoordinatesDatabasemodel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_PladserDatabasemodel_pladserId",
                table: "InstitutionFrontPageModel",
                column: "pladserId",
                principalTable: "PladserDatabasemodel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_AddressDatabasemodel_addressId",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_InstKoordinatesDatabasemodel_KoordinatesId",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_PladserDatabasemodel_pladserId",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropTable(
                name: "AddressDatabasemodel");

            migrationBuilder.DropTable(
                name: "InstitutionTilsynsRapportDatabasemodel");

            migrationBuilder.DropTable(
                name: "InstKoordinatesDatabasemodel");

            migrationBuilder.DropTable(
                name: "PladserDatabasemodel");

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vej = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip_code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionTilsynsRapport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstitutionFrontPageModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    documentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionTilsynsRapport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionTilsynsRapport_InstitutionFrontPageModel_InstitutionFrontPageModelId",
                        column: x => x.InstitutionFrontPageModelId,
                        principalTable: "InstitutionFrontPageModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InstKoordinates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    lng = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstKoordinates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pladser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoernehavePladser = table.Column<int>(type: "int", nullable: false),
                    VuggestuePladser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pladser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionTilsynsRapport_InstitutionFrontPageModelId",
                table: "InstitutionTilsynsRapport",
                column: "InstitutionFrontPageModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_Address_addressId",
                table: "InstitutionFrontPageModel",
                column: "addressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_InstKoordinates_KoordinatesId",
                table: "InstitutionFrontPageModel",
                column: "KoordinatesId",
                principalTable: "InstKoordinates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_Pladser_pladserId",
                table: "InstitutionFrontPageModel",
                column: "pladserId",
                principalTable: "Pladser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
