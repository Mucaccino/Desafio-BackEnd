using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motto.Data.Entities;

/// <summary>
/// RentalPlan class
/// </summary>
[Table("RentalPlans")]
public class RentalPlan
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Days is required")]
    public int Days { get; set; }

    [Required(ErrorMessage = "DailyCost is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DailyCost { get; set; }
}
