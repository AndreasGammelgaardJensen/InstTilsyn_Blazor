﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VuggestueTilsynScraper.Database;

#nullable disable

namespace VuggestueTilsynScraper.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230906221913_createdAtAndLastCreatedBy")]
    partial class createdAtAndLastCreatedBy
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ModelsLib.DatabaseModels.AddressDatabasemodel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Vej")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Zip_code")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AddressDatabasemodel");
                });

            modelBuilder.Entity("ModelsLib.DatabaseModels.InstKoordinatesDatabasemodel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("lat")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("lng")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("InstKoordinatesDatabasemodel");
                });

            modelBuilder.Entity("ModelsLib.DatabaseModels.InstitutionFrontPageModelDatabasemodel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("KoordinatesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeEnum")
                        .HasColumnType("int");

                    b.Property<Guid>("addressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("homePage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("pladserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("profile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("KoordinatesId");

                    b.HasIndex("addressId");

                    b.HasIndex("pladserId");

                    b.ToTable("InstitutionFrontPageModel");
                });

            modelBuilder.Entity("ModelsLib.DatabaseModels.InstitutionTilsynsRapportDatabasemodel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("InstitutionFrontPageModelDatabasemodelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("copyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("documentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fileUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionFrontPageModelDatabasemodelId");

                    b.ToTable("InstitutionTilsynsRapportDatabasemodel");
                });

            modelBuilder.Entity("ModelsLib.DatabaseModels.PladserDatabasemodel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BoernehavePladser")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DagplejePladser")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("VuggestuePladser")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PladserDatabasemodel");
                });

            modelBuilder.Entity("ModelsLib.DatabaseModels.InstitutionFrontPageModelDatabasemodel", b =>
                {
                    b.HasOne("ModelsLib.DatabaseModels.InstKoordinatesDatabasemodel", "Koordinates")
                        .WithMany()
                        .HasForeignKey("KoordinatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsLib.DatabaseModels.AddressDatabasemodel", "address")
                        .WithMany()
                        .HasForeignKey("addressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsLib.DatabaseModels.PladserDatabasemodel", "pladser")
                        .WithMany()
                        .HasForeignKey("pladserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Koordinates");

                    b.Navigation("address");

                    b.Navigation("pladser");
                });

            modelBuilder.Entity("ModelsLib.DatabaseModels.InstitutionTilsynsRapportDatabasemodel", b =>
                {
                    b.HasOne("ModelsLib.DatabaseModels.InstitutionFrontPageModelDatabasemodel", null)
                        .WithMany("InstitutionTilsynsRapports")
                        .HasForeignKey("InstitutionFrontPageModelDatabasemodelId");
                });

            modelBuilder.Entity("ModelsLib.DatabaseModels.InstitutionFrontPageModelDatabasemodel", b =>
                {
                    b.Navigation("InstitutionTilsynsRapports");
                });
#pragma warning restore 612, 618
        }
    }
}
