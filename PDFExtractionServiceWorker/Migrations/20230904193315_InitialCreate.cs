﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDFExtractionServiceWorker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstitutionReportCriterieaDatabasemodel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionReportCriterieaDatabasemodel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_CategoriClass_InstitutionReportCriterieaDatabasemodelId",
                table: "CategoriClass",
                column: "InstitutionReportCriterieaDatabasemodelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriClass");

            migrationBuilder.DropTable(
                name: "InstitutionReportCriterieaDatabasemodel");
        }
    }
}
