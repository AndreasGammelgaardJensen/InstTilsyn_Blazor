using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ContactsTODb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContactDatabasemodelID",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ContactDatabasemodel",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomePage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDatabasemodel", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionFrontPageModel_ContactDatabasemodelID",
                table: "InstitutionFrontPageModel",
                column: "ContactDatabasemodelID");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_ContactDatabasemodel_ContactDatabasemodelID",
                table: "InstitutionFrontPageModel",
                column: "ContactDatabasemodelID",
                principalTable: "ContactDatabasemodel",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_ContactDatabasemodel_ContactDatabasemodelID",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropTable(
                name: "ContactDatabasemodel");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionFrontPageModel_ContactDatabasemodelID",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropColumn(
                name: "ContactDatabasemodelID",
                table: "InstitutionFrontPageModel");
        }
    }
}
