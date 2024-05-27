using Motto.Utils;

namespace Motto.Models;

public enum UserType
{
    Admin,
    DeliveryDriver
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
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
    public string? RefreshToken { get; set; }
}

public class Admin : User
{
    // Propriedades específicas para o tipo Admin, se necessário
}