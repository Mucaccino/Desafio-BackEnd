using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motto.Entities;

/// <summary>
/// Motorcycle class
/// </summary>
[Table("Motorcycles")]
[Index(nameof(Plate), IsUnique = true)]
public class Motorcycle
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Year is required")]
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Model is required")]
    [StringLength(100, ErrorMessage = "Model must be at most 100 characters long")]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "Plate is required")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "Plate must be exactly 8 characters long")]
    [RegularExpression(@"^[A-Z]{3}-[0-9]{4}$", ErrorMessage = "Invalid plate format")]
    [Column(TypeName = "char(8)")]
    public string Plate { get; set; } = string.Empty;
}
