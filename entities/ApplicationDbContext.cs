using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
using Motto.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<DeliveryDriver> DeliveryDrivers { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<MotorcycleRegisteredEvent> MotorcycleRegisteredEvents { get; set; }
    public DbSet<MotorcycleYear2024Message> MotorcycleYear2024Messages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure the connection string for PostgreSQL
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=mottodb;User Id=motouser;Password=motopassword;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent configurations
        modelBuilder.Entity<Motorcycle>(ConfigureMotorcycle);
        modelBuilder.Entity<DeliveryDriver>(ConfigureDeliveryDriver);
        modelBuilder.Entity<Rental>(ConfigureRental);
        modelBuilder.Entity<MotorcycleRegisteredEvent>(ConfigureMotorcycleRegisteredEvent);
        modelBuilder.Entity<MotorcycleYear2024Message>(ConfigureMotorcycleYear2024Message);
    }

    private void ConfigureMotorcycle(EntityTypeBuilder<Motorcycle> builder)
    {
        // Configure Motorcycle entity
        builder.HasKey(m => m.Identifier);
        builder.Property(m => m.Model).IsRequired();
        builder.HasIndex(m => m.Plate).IsUnique();
    }

    private void ConfigureDeliveryDriver(EntityTypeBuilder<DeliveryDriver> builder)
    {
        // Configure DeliveryDriver entity
        builder.HasKey(d => d.Identifier);
        builder.Property(d => d.Name).IsRequired();
        builder.HasIndex(d => d.CNPJ).IsUnique();
        builder.HasIndex(d => d.DriverLicenseNumber).IsUnique();
    }

    private void ConfigureRental(EntityTypeBuilder<Rental> builder)
    {
        // Configure Rental entity
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.Motorcycle).WithMany().HasForeignKey(r => r.MotorcycleId);
        builder.HasOne(r => r.DeliveryDriver).WithMany().HasForeignKey(r => r.DeliveryDriverId);
    }

    private void ConfigureMotorcycleRegisteredEvent(EntityTypeBuilder<MotorcycleRegisteredEvent> builder)
    {
        // Configure MotorcycleRegisteredEvent entity
        builder.HasKey(e => e.Id);
    }

    private void ConfigureMotorcycleYear2024Message(EntityTypeBuilder<MotorcycleYear2024Message> builder)
    {
        // Configure MotorcycleYear2024Message entity
        builder.HasKey(m => m.Id);
    }
}
