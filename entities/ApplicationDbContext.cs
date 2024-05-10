using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
using Motto.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; } // Adicionando o DbSet para a classe User
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<DeliveryDriver> DeliveryDrivers { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<RentalPlan> RentalPlans { get; set; } 
    public DbSet<MotorcycleEvent> MotorcycleEvents { get; set; }
    public DbSet<MotorcycleMessage> MotorcycleMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure the connection string for PostgreSQL
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=mottodb;User Id=mottouser;Password=mottopassword;");
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
        builder.Property(u => u.Username).IsRequired();
        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.Type).IsRequired();
    }

    private void ConfigureMotorcycle(EntityTypeBuilder<Motorcycle> builder)
    {
        // Configure Motorcycle entity
        builder.ToTable("Motorcycle");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Model).IsRequired();
        builder.Property(m => m.Plate).IsRequired();
        builder.HasIndex(m => m.Plate).IsUnique();
    }

    private void ConfigureDeliveryDriver(EntityTypeBuilder<DeliveryDriver> builder)
    {
        // Configure DeliveryDriver entity
        builder.ToTable("DeliveryDriver");
        builder.HasBaseType<User>();
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
