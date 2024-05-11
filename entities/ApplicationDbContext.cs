using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Motto.Models;

namespace Motto.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } // Adicionando o DbSet para a classe User
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<DeliveryDriver> DeliveryDrivers { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<RentalPlan> RentalPlans { get; set; }
    public DbSet<MotorcycleEvent> MotorcycleEvents { get; set; }
    public DbSet<MotorcycleMessage> MotorcycleMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        // Configure the connection string for PostgreSQL
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent configurations
        modelBuilder.Entity<User>(ConfigureUser);
        modelBuilder.Entity<Motorcycle>(ConfigureMotorcycle);
        modelBuilder.Entity<DeliveryDriver>(ConfigureDeliveryDriver);
        modelBuilder.Entity<Rental>(ConfigureRental);
        modelBuilder.Entity<RentalPlan>(ConfigureRentalPlan);
        modelBuilder.Entity<MotorcycleEvent>(ConfigureMotorcycleEvent);
        modelBuilder.Entity<MotorcycleMessage>(ConfigureMotorcycleMessage);

        // Seed data
        SeedData.Seed(modelBuilder);
    }

    private void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        // Configure User entity
        builder.ToTable("User");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Salt).IsRequired();
        builder.Property(u => u.Type).IsRequired();
    }

    private void ConfigureMotorcycle(EntityTypeBuilder<Motorcycle> builder)
    {
        // Configure Motorcycle entity
        builder.ToTable("Motorcycle", b =>
            {
                b.HasCheckConstraint("CK_Motorcycle_Plate_Format", "\"Plate\" ~ '[A-Z]{3}-[0-9]{4}'");
            });
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Model)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(m => m.Year)
            .IsRequired()
            .HasColumnType("int");
        builder.Property(m => m.Plate)
            .IsRequired()
            .HasMaxLength(8)
            .IsFixedLength()
            .IsUnicode(false);
    }

    private void ConfigureDeliveryDriver(EntityTypeBuilder<DeliveryDriver> builder)
    {
        // Configure DeliveryDriver entity
        builder.ToTable("DeliveryDriver", b =>
        {
            b.HasCheckConstraint("CK_DeliveryDriver_DriverLicenseType_Format", "\"DriverLicenseType\" IN ('A', 'B', 'AB')");
            b.HasCheckConstraint("CK_DeliveryDriver_CNPJ_Format", "\"CNPJ\" ~ '[0-9]{14}'");
            b.HasCheckConstraint("CK_DeliveryDriver_DriverLicenseNumber_Format", "\"DriverLicenseNumber\" ~ '[0-9]{11}'");
        });
        builder.HasBaseType<User>();
        builder.Property(d => d.DateOfBirth).IsRequired().HasColumnType("date");
        builder.Property(d => d.CNPJ).IsRequired();
        builder.Property(d => d.DriverLicenseNumber).IsRequired();
        builder.HasIndex(d => d.CNPJ).IsUnique();
        builder.HasIndex(d => d.DriverLicenseNumber).IsUnique();
    }

    private void ConfigureRental(EntityTypeBuilder<Rental> builder)
    {
        // Configure Rental entity
        builder.ToTable("Rental");
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.Motorcycle).WithMany().HasForeignKey(r => r.MotorcycleId);
        builder.HasOne(r => r.DeliveryDriver).WithMany().HasForeignKey(r => r.DeliveryDriverId);
        builder.Property(r => r.StartDate).IsRequired().HasColumnType("date");
        builder.Property(r => r.EndDate).IsRequired().HasColumnType("date");
        builder.Property(r => r.ExpectedEndDate).IsRequired().HasColumnType("date");
    }

    private void ConfigureRentalPlan(EntityTypeBuilder<RentalPlan> builder)
    {
        // Configure RentalPlan entity
        builder.ToTable("RentalPlan");
        builder.HasKey(rp => rp.Id);
        builder.Property(rp => rp.Days).IsRequired();
        builder.Property(rp => rp.DailyCost).HasColumnType("decimal(18,2)").IsRequired();
    }

    private void ConfigureMotorcycleEvent(EntityTypeBuilder<MotorcycleEvent> builder)
    {
        // Configure MotorcycleEvent entity
        builder.ToTable("MotorcycleEvent");
        builder.HasKey(e => e.Id);
    }

    private void ConfigureMotorcycleMessage(EntityTypeBuilder<MotorcycleMessage> builder)
    {
        // Configure MotorcycleMessage entity
        builder.ToTable("MotorcycleMessage");
        builder.HasKey(m => m.Id);
    }
}