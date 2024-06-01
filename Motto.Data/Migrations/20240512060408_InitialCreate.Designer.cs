﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Motto.Data;

#nullable disable

namespace entities.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240512060408_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                        .IsUnicode(false)
                        .HasColumnType("character(8)")
                        .IsFixedLength();

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Motorcycle", null, t =>
                        {
                            t.HasCheckConstraint("CK_Motorcycle_Plate_Format", "\"Plate\" ~ '[A-Z]{3}-[0-9]{4}'");
                        });

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Model = "Halley Davidson",
                            Plate = "AAA-1234",
                            Year = 1985
                        },
                        new
                        {
                            Id = 2,
                            Model = "Honda",
                            Plate = "AAA-4321",
                            Year = 1995
                        });
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

                    b.ToTable("MotorcycleEvent", (string)null);
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

                    b.ToTable("MotorcycleMessage", (string)null);
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
                        .HasColumnType("date");

                    b.Property<DateTime>("ExpectedEndDate")
                        .HasColumnType("date");

                    b.Property<int>("MotorcycleId")
                        .HasColumnType("integer");

                    b.Property<decimal>("PenaltyCost")
                        .HasColumnType("numeric");

                    b.Property<int>("RentalPlanId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryDriverId");

                    b.HasIndex("MotorcycleId");

                    b.HasIndex("RentalPlanId");

                    b.ToTable("Rental", (string)null);
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

                    b.ToTable("RentalPlan", (string)null);

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
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

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

                    b.ToTable("User", (string)null);

                    b.UseTptMappingStrategy();

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Usuário Administrador",
                            PasswordHash = "IWsJIfH/pPqlLPHjzDLxGVWVjpvAM+ReG1hV65gXSL0=",
                            Salt = "bKeBJ/CYO+3AM7y6G1qncw==",
                            Type = 0,
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("Motto.Entities.DeliveryDriver", b =>
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

                    b.ToTable("DeliveryDriver", null, t =>
                        {
                            t.HasCheckConstraint("CK_DeliveryDriver_CNPJ_Format", "\"CNPJ\" ~ '[0-9]{14}'");

                            t.HasCheckConstraint("CK_DeliveryDriver_DriverLicenseNumber_Format", "\"DriverLicenseNumber\" ~ '[0-9]{11}'");

                            t.HasCheckConstraint("CK_DeliveryDriver_DriverLicenseType_Format", "\"DriverLicenseType\" IN ('A', 'B', 'AB')");
                        });

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Name = "Usuário Entregador",
                            PasswordHash = "Cz8m4JWkmIeRUnqJX2RA5GFCRwqQ/Dkj8UPXUXYy5Zw=",
                            Salt = "NGvULlho+9JkN5IhxAOIOw==",
                            Type = 1,
                            Username = "entregador",
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
                    b.HasOne("Motto.Entities.DeliveryDriver", "DeliveryDriver")
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

            modelBuilder.Entity("Motto.Entities.DeliveryDriver", b =>
                {
                    b.HasOne("Motto.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("Motto.Entities.DeliveryDriver", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
