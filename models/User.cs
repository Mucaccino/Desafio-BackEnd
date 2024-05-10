using Motto.Utils;

namespace Motto.Models;

public enum UserType
{
    Admin,
    DeliveryDriver
}

/*
// to create a new user
var user = new User
{
    Username = "userx",
};
user.SetPassword("password123");

// to verify password
var user = dbContext.Users.FirstOrDefault(u => u.Username == "alice");
if (user != null && user.VerifyPassword("password123"))
{
    // Senha correta
}
*/

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string? Name { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    
    public void SetPassword(string password)
    {
        (PasswordHash, Salt) = PasswordHasher.HashPassword(password);
    }
    public bool VerifyPassword(string password)
    {
        return PasswordHasher.VerifyPassword(password, Salt, PasswordHash);
    }
    public UserType Type { get; set; }
}

public class Admin : User
{
    // Propriedades específicas para o tipo Admin, se necessário
}