using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_ContactDatabasemodel_ContactDatabasemodelID",
                table: "InstitutionFrontPageModel");

            migrationBuilder.RenameColumn(
                name: "ContactDatabasemodelID",
                table: "InstitutionFrontPageModel",
                newName: "ContactDatabasemodelId");

            migrationBuilder.RenameIndex(
                name: "IX_InstitutionFrontPageModel_ContactDatabasemodelID",
                table: "InstitutionFrontPageModel",
                newName: "IX_InstitutionFrontPageModel_ContactDatabasemodelId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ContactDatabasemodel",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactDatabasemodelId",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "ContactDatabasemodel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "HomePage",
                table: "ContactDatabasemodel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ContactDatabasemodel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ContactDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedAt",
                table: "ContactDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_ContactDatabasemodel_ContactDatabasemodelId",
                table: "InstitutionFrontPageModel",
                column: "ContactDatabasemodelId",
                principalTable: "ContactDatabasemodel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionFrontPageModel_ContactDatabasemodel_ContactDatabasemodelId",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ContactDatabasemodel");

            migrationBuilder.DropColumn(
                name: "LastChangedAt",
                table: "ContactDatabasemodel");

            migrationBuilder.RenameColumn(
                name: "ContactDatabasemodelId",
                table: "InstitutionFrontPageModel",
                newName: "ContactDatabasemodelID");

            migrationBuilder.RenameIndex(
                name: "IX_InstitutionFrontPageModel_ContactDatabasemodelId",
                table: "InstitutionFrontPageModel",
                newName: "IX_InstitutionFrontPageModel_ContactDatabasemodelID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ContactDatabasemodel",
                newName: "ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactDatabasemodelID",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "ContactDatabasemodel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HomePage",
                table: "ContactDatabasemodel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ContactDatabasemodel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_ContactDatabasemodel_ContactDatabasemodelID",
                table: "InstitutionFrontPageModel",
                column: "ContactDatabasemodelID",
                principalTable: "ContactDatabasemodel",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
