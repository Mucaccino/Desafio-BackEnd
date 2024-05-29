using Motto.Enums;
using Motto.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motto.Entities;

/// <summary>
/// User class
/// </summary>
[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Password hash is required")]
    public string PasswordHash { get; set; } = string.Empty;

    [Required(ErrorMessage = "Salt is required")]
    public string Salt { get; set; } = string.Empty;

    public void SetPassword(string password)
    {
        (PasswordHash, Salt) = PasswordHasher.HashPassword(password);
    }

    public bool VerifyPassword(string password)
    {
        return PasswordHasher.VerifyPassword(password, Salt, PasswordHash);
    }

    [Required(ErrorMessage = "User type is required")]
    public UserType Type { get; set; }

    [StringLength(200, ErrorMessage = "RefreshToken cannot be longer than 200 characters")]
    public string? RefreshToken { get; set; }
}

public class AdminUser : User
{
    // only if has admin implementations here
}