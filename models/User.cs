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
    public string Password { get; set; }
    public string? Name { get; set; }
    public UserType Type { get; set; }
}

public class Admin : User
{
    // Propriedades específicas para o tipo Admin, se necessário
}