﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Motto.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace entities.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Motto.Entities.Motorcycle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("char(8)");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Plate")
                        .IsUnique();

                    b.ToTable("Motorcycles");
                });

            modelBuilder.Entity("Motto.Entities.MotorcycleEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MotorcycleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MotorcycleId");

                    b.ToTable("MotorcycleEvents");
                });

            modelBuilder.Entity("Motto.Entities.MotorcycleMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("MotorcycleMessages");
                });

            modelBuilder.Entity("Motto.Entities.Rental", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DeliveryDriverId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpectedEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MotorcycleId")
                        .HasColumnType("integer");

                    b.Property<decimal>("PenaltyCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RentalPlanId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryDriverId");

                    b.HasIndex("MotorcycleId");

                    b.HasIndex("RentalPlanId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("Motto.Entities.RentalPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("DailyCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Days")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("RentalPlans");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DailyCost = 30.00m,
                            Days = 7
                        },
                        new
                        {
                            Id = 2,
                            DailyCost = 28.00m,
                            Days = 15
                        },
                        new
                        {
                            Id = 3,
                            DailyCost = 22.00m,
                            Days = 30
                        },
                        new
                        {
                            Id = 4,
                            DailyCost = 20.00m,
                            Days = 45
                        },
                        new
                        {
                            Id = 5,
                            DailyCost = 18.00m,
                            Days = 50
                        });
                });

            modelBuilder.Entity("Motto.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.UseTptMappingStrategy();

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Administrador",
                            PasswordHash = "RbF6T+NE4VY2LhOYI2tAeb7PNFxIwhq0VVPDLRu/nig=",
                            Salt = "vHcSA5HnmKGt0RzUDUD3Uw==",
                            Type = 0,
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("Motto.Entities.DeliveryDriverUser", b =>
                {
                    b.HasBaseType("Motto.Entities.User");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("DriverLicenseImage")
                        .HasColumnType("text");

                    b.Property<string>("DriverLicenseNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DriverLicenseType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasIndex("CNPJ")
                        .IsUnique();

                    b.HasIndex("DriverLicenseNumber")
                        .IsUnique();

                    b.ToTable("DeliveryDrivers");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Name = "Motoboy",
                            PasswordHash = "5xO4CxGENty1RE7aahNgnUbFFK/AzJX3yaFsaLz/FAk=",
                            Salt = "d2lZVq+wguqtVvahL/f+oQ==",
                            Type = 1,
                            Username = "motoboy",
                            CNPJ = "12345678901234",
                            DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DriverLicenseNumber = "12345678901",
                            DriverLicenseType = "AB"
                        });
                });

            modelBuilder.Entity("Motto.Entities.MotorcycleEvent", b =>
                {
                    b.HasOne("Motto.Entities.Motorcycle", "Motorcycle")
                        .WithMany()
                        .HasForeignKey("MotorcycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Motorcycle");
                });

            modelBuilder.Entity("Motto.Entities.Rental", b =>
                {
                    b.HasOne("Motto.Entities.DeliveryDriverUser", "DeliveryDriver")
                        .WithMany()
                        .HasForeignKey("DeliveryDriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Motto.Entities.Motorcycle", "Motorcycle")
                        .WithMany()
                        .HasForeignKey("MotorcycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Motto.Entities.RentalPlan", "RentalPlan")
                        .WithMany()
                        .HasForeignKey("RentalPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeliveryDriver");

                    b.Navigation("Motorcycle");

                    b.Navigation("RentalPlan");
                });

            modelBuilder.Entity("Motto.Entities.DeliveryDriverUser", b =>
                {
                    b.HasOne("Motto.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("Motto.Entities.DeliveryDriverUser", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
