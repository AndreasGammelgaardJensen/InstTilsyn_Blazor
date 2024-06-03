using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressDatabasemodel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vej = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip_code = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressDatabasemodel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionReportCriterieaDatabasemodel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionReportCriterieaDatabasemodel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstKoordinatesDatabasemodel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    lng = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VuggestuePladser = table.Column<int>(type: "int", nullable: false),
                    BoernehavePladser = table.Column<int>(type: "int", nullable: false),
                    DagplejePladser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PladserDatabasemodel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoriText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indsats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstitutionReportCriterieaDatabasemodelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriClass_InstitutionReportCriterieaDatabasemodel_InstitutionReportCriterieaDatabasemodelId",
                        column: x => x.InstitutionReportCriterieaDatabasemodelId,
                        principalTable: "InstitutionReportCriterieaDatabasemodel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InstitutionFrontPageModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeEnum = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    addressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    pladserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    homePage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    profile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KoordinatesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionFrontPageModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionFrontPageModel_AddressDatabasemodel_addressId",
                        column: x => x.addressId,
                        principalTable: "AddressDatabasemodel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InstitutionFrontPageModel_InstKoordinatesDatabasemodel_KoordinatesId",
                        column: x => x.KoordinatesId,
                        principalTable: "InstKoordinatesDatabasemodel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InstitutionFrontPageModel_PladserDatabasemodel_pladserId",
                        column: x => x.pladserId,
                        principalTable: "PladserDatabasemodel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InstitutionTilsynsRapportDatabasemodel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    documentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    copyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_CategoriClass_InstitutionReportCriterieaDatabasemodelId",
                table: "CategoriClass",
                column: "InstitutionReportCriterieaDatabasemodelId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionFrontPageModel_addressId",
                table: "InstitutionFrontPageModel",
                column: "addressId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionFrontPageModel_KoordinatesId",
                table: "InstitutionFrontPageModel",
                column: "KoordinatesId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionFrontPageModel_pladserId",
                table: "InstitutionFrontPageModel",
                column: "pladserId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionTilsynsRapportDatabasemodel_InstitutionFrontPageModelDatabasemodelId",
                table: "InstitutionTilsynsRapportDatabasemodel",
                column: "InstitutionFrontPageModelDatabasemodelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriClass");

            migrationBuilder.DropTable(
                name: "InstitutionTilsynsRapportDatabasemodel");

            migrationBuilder.DropTable(
                name: "InstitutionReportCriterieaDatabasemodel");

            migrationBuilder.DropTable(
                name: "InstitutionFrontPageModel");

            migrationBuilder.DropTable(
                name: "AddressDatabasemodel");

            migrationBuilder.DropTable(
                name: "InstKoordinatesDatabasemodel");

            migrationBuilder.DropTable(
                name: "PladserDatabasemodel");
        }
    }
}
