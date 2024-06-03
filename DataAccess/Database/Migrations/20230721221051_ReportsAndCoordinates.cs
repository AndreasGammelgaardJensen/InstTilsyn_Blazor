using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VuggestueTilsynScraper.Migrations
{
    /// <inheritdoc />
    public partial class ReportsAndCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "KoordinatesId",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "InstitutionTilsynsRapport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    documentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstitutionFrontPageModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionFrontPageModel_KoordinatesId",
                table: "InstitutionFrontPageModel",
                column: "KoordinatesId");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionTilsynsRapport_InstitutionFrontPageModelId",
                table: "InstitutionTilsynsRapport",
                column: "InstitutionFrontPageModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_InstKoordinates_KoordinatesId",
                table: "InstitutionFrontPageModel",
                column: "KoordinatesId",
                principalTable: "InstKoordinates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_InstKoordinates_KoordinatesId",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropTable(
                name: "InstitutionTilsynsRapport");

            migrationBuilder.DropTable(
                name: "InstKoordinates");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionFrontPageModel_KoordinatesId",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropColumn(
                name: "KoordinatesId",
                table: "InstitutionFrontPageModel");
        }
    }
}
