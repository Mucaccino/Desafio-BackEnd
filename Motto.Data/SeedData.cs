using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Enums;
using Motto.Utils;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Seed Users
        var password = PasswordHasher.HashPassword("123mudar");
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Administrador", Username = "admin",
                PasswordHash = password.HashedPassword,
                Salt = password.Salt, Type = UserType.Admin }
        );

        password = PasswordHasher.HashPassword("123mudar");
        modelBuilder.Entity<DeliveryDriverUser>().HasData(
            new DeliveryDriverUser { Id = 2, Name = "Entregador", Username = "entregador",
                PasswordHash = password.HashedPassword,
                Salt = password.Salt, Type = UserType.DeliveryDriver,
                DriverLicenseNumber = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1),
                DriverLicenseType = DriverLicenseType.AB.ToString(),
                CNPJ = "12345678901234" }
        );

        // Seed RentalPlans
        modelBuilder.Entity<RentalPlan>().HasData(
            new RentalPlan { Id = 1, Days = 7, DailyCost = 30.00m },
            new RentalPlan { Id = 2, Days = 15, DailyCost = 28.00m },
            new RentalPlan { Id = 3, Days = 30, DailyCost = 22.00m },
            new RentalPlan { Id = 4, Days = 45, DailyCost = 20.00m },
            new RentalPlan { Id = 5, Days = 50, DailyCost = 18.00m }
        );
    }
}
