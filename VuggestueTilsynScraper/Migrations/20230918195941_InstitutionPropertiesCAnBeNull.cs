using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VuggestueTilsynScraper.Migrations
{
    /// <inheritdoc />
    public partial class InstitutionPropertiesCAnBeNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "profile",
                table: "InstitutionFrontPageModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "pladserId",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "homePage",
                table: "InstitutionFrontPageModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "addressId",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "KoordinatesId",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_AddressDatabasemodel_addressId",
                table: "InstitutionFrontPageModel",
                column: "addressId",
                principalTable: "AddressDatabasemodel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_InstKoordinatesDatabasemodel_KoordinatesId",
                table: "InstitutionFrontPageModel",
                column: "KoordinatesId",
                principalTable: "InstKoordinatesDatabasemodel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionFrontPageModel_PladserDatabasemodel_pladserId",
                table: "InstitutionFrontPageModel",
                column: "pladserId",
                principalTable: "PladserDatabasemodel",
                principalColumn: "Id");
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

            migrationBuilder.AlterColumn<string>(
                name: "profile",
                table: "InstitutionFrontPageModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "pladserId",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "homePage",
                table: "InstitutionFrontPageModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "addressId",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "KoordinatesId",
                table: "InstitutionFrontPageModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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
    }
}
