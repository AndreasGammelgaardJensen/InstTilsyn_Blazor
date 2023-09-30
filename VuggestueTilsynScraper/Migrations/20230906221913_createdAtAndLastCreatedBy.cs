using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VuggestueTilsynScraper.Migrations
{
    /// <inheritdoc />
    public partial class createdAtAndLastCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PladserDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedAt",
                table: "PladserDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "InstKoordinatesDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedAt",
                table: "InstKoordinatesDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "InstitutionTilsynsRapportDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedAt",
                table: "InstitutionTilsynsRapportDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "InstitutionFrontPageModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedAt",
                table: "InstitutionFrontPageModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AddressDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedAt",
                table: "AddressDatabasemodel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PladserDatabasemodel");

            migrationBuilder.DropColumn(
                name: "LastChangedAt",
                table: "PladserDatabasemodel");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "InstKoordinatesDatabasemodel");

            migrationBuilder.DropColumn(
                name: "LastChangedAt",
                table: "InstKoordinatesDatabasemodel");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "InstitutionTilsynsRapportDatabasemodel");

            migrationBuilder.DropColumn(
                name: "LastChangedAt",
                table: "InstitutionTilsynsRapportDatabasemodel");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropColumn(
                name: "LastChangedAt",
                table: "InstitutionFrontPageModel");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AddressDatabasemodel");

            migrationBuilder.DropColumn(
                name: "LastChangedAt",
                table: "AddressDatabasemodel");
        }
    }
}
