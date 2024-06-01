using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Motto.Data.Entities;
using Serilog;

namespace Motto.Data;

/// <summary>
/// ApplicationDbContext class
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Motorcycle> Motorcycles { get; set; }
    public virtual DbSet<DeliveryDriverUser> DeliveryDriverUsers { get; set; }
    public virtual DbSet<Rental> Rentals { get; set; }
    public virtual DbSet<RentalPlan> RentalPlans { get; set; }
    public virtual DbSet<MotorcycleEvent> MotorcycleEvents { get; set; }
    public virtual DbSet<MotorcycleMessage> MotorcycleMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables() // Adicione essa linha para incluir as variáveis de ambiente
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true);
        var configuration = builder.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Log.Information("Attempting to configure the database with connection string: {ConnectionString}", connectionString);
        optionsBuilder.UseNpgsql(connectionString);
        Log.Information("Database configuration successful");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed initial data
        SeedData.Seed(modelBuilder);
    }
}