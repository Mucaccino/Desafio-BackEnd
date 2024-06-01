using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motto.Data.Entities;

/// <summary>
/// DeliveryDriverUser class
/// </summary>
[Table("DeliveryDrivers")]
[Index("CNPJ", IsUnique = true)]
[Index("DriverLicenseNumber", IsUnique = true)]
public class DeliveryDriverUser : User
{

    [Required(ErrorMessage = "CNPJ is required")]
    [RegularExpression(@"^\d{2}\.?\d{3}\.?\d{3}/?\d{4}-?\d{2}$|^\d{14}$", ErrorMessage = "Invalid CNPJ format")]
    public string CNPJ { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    [Column(TypeName = "date")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Driver license number is required")]
    [RegularExpression(@"^\d{2}\.?\d{3}\.?\d{3}-?\d{2}$|^\d{11}$", ErrorMessage = "Invalid driver license number format")]
    public string DriverLicenseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Driver license type is required")]
    [RegularExpression(@"^(A|B|AB)$", ErrorMessage = "Invalid driver license type format")]
    public string DriverLicenseType { get; set; } = string.Empty;

    public string? DriverLicenseImage { get; set; } // Path of license image
}