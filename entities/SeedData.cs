using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motto.Models;
using System;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Admin", Username = "admin", Password = "123admin", Type = UserType.Admin },
            new User { Id = 2, Name = "Entregador", Username = "entregador", Password = "123entregador", Type = UserType.DeliveryDriver }
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
