

using Motto.Models;

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
                    Id = 1,
                    Name = "Entregador",
                    Username = "entregador",
                    Type = UserType.Admin,
                    PasswordHash = "pnmPf+pm8LSQo80S7Fw258DJPP+EDqvBzdSQtN3YV70=",
                    Salt = "KghpVaPmUZQUOQ+xXmHjWA=="
                }
            };
    }
}