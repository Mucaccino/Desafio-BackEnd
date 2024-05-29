

using Motto.Entities;
using Motto.Enums;

namespace Motto.Tests;

public class TestDataHelper
{
    internal static string GetJwtToken()
    {
        return "XuK8wIRekSwZ5Yb4QSs/Rdq9a9tFifxZCSCOldWtQ15S5qzf+e7OwuKoW5/mb0dKDKTWc4/myvkrsINqlFcmXg==";
    }

    internal static List<User> GetFakeUserList()
    {
        return new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Username = "admin",
                    Type = UserType.Admin,
                    PasswordHash = "/NaJoraYfWOP3f7b07jgZTWOu/s0w7xG9SC4MO8uY3w=",
                    Salt = "TiOBSKmIquQn42zQYaqa3w=="
                },
                new User
                {
                    Id = 2,
                    Name = "Entregador",
                    Username = "entregador",
                    Type = UserType.DeliveryDriver,
                    PasswordHash = "pnmPf+pm8LSQo80S7Fw258DJPP+EDqvBzdSQtN3YV70=",
                    Salt = "KghpVaPmUZQUOQ+xXmHjWA=="
                },
                new User
                {
                    Id = 3,
                    Name = "Entregador 2",
                    Username = "entregador2",
                    Type = UserType.DeliveryDriver,
                    PasswordHash = "pnmPf+pm8LSQo80S7Fw258DJPP+EDqvBzdSQtN3YV70=",
                    Salt = "KghpVaPmUZQUOQ+xXmHjWA=="
                }
            };
    }

    internal static List<RentalPlan> GetFakeRentalPlanList()
    {
        return new List<RentalPlan>
            {
                new RentalPlan
                {
                    Id = 1,
                    Days = 7,
                    DailyCost = 30.00m
                },
                new RentalPlan
                {
                    Id = 2,
                    Days = 15,
                    DailyCost = 28.00m
                },
                new RentalPlan
                {
                    Id = 3,
                    Days = 30,
                    DailyCost = 22.00m  
                },                
                new RentalPlan
                {
                    Id = 4,
                    Days = 45,
                    DailyCost = 20.00m
                },
                new RentalPlan
                {
                    Id = 5,
                    Days = 50,
                    DailyCost = 18.00m
                }
            };  
    }

    internal static IEnumerable<DeliveryDriverUser> GetFakeDeliveryDriverList()
    {
        return new List<DeliveryDriverUser>
            {
                new DeliveryDriverUser
                {
                    Id = 2,
                    Name = "Entregador",
                    Username = "entregador",
                    PasswordHash = "pnmPf+pm8LSQo80S7Fw258DJPP+EDqvBzdSQtN3YV70=",
                    Salt = "KghpVaPmUZQUOQ+xXmHjWA==",
                    Type = UserType.DeliveryDriver,
                    DriverLicenseNumber = "12345678901",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    DriverLicenseType = DriverLicenseType.AB.ToString(),
                    CNPJ = "12345678901234"
                },
                new DeliveryDriverUser
                {
                    Id = 3,
                    Name = "Entregador 2",
                    Username = "entregador2",
                    PasswordHash = "pnmPf+pm8LSQo80S7Fw258DJPP+EDqvBzdSQtN3YV70=",
                    Salt = "KghpVaPmUZQUOQ+xXmHjWA==",
                    Type = UserType.DeliveryDriver,
                    DriverLicenseNumber = "12345678901",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    DriverLicenseType = DriverLicenseType.B.ToString(),
                    CNPJ = "12345678901234"
                }
            };
    }

    internal static List<Rental> GetFakeRentalList()
    {
        return new List<Rental>
            {
                // new Rental
                // {
                //     Id = 1,
                //     DeliveryDriverId = 2,
                //     MotorcycleId = 1,
                //     RentalPlanId = 2,
                //     StartDate = DateTime.Today.AddDays(-5),
                //     ExpectedEndDate = DateTime.Today.AddDays(2)
                // }
            };
    }   

    internal static List<Motorcycle> GetFakeMotorcycleList()
    {
        return new List<Motorcycle>
            {
                // new Motorcycle
                // {
                //     Id = 1,
                //     Year = 2010,
                //     Plate = "ABC1234",
                //     Model = "CB 300",    
                // },
                // new Motorcycle
                // {
                //     Id = 2,
                //     Year = 2012,
                //     Plate = "ABC3234",
                //     Model = "CB 400",    
                // }   
            };
    }
}