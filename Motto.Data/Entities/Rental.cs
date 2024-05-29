using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motto.Entities;

/// <summary>
/// Rental class
/// </summary>
[Table("Rentals")]
public class Rental
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "MotorcycleId is required")]
    public int MotorcycleId { get; set; } // ForeignKey to Motorcycle

    [Required(ErrorMessage = "DeliveryDriverId is required")]
    public int DeliveryDriverId { get; set; } // ForeignKey to DeliveryDriver

    [Required(ErrorMessage = "RentalPlanId is required")]
    public int RentalPlanId { get; set; } // ForeignKey to RentalPlan

    [ForeignKey("MotorcycleId")]
    public Motorcycle? Motorcycle { get; set; }

    [ForeignKey("DeliveryDriverId")]
    public DeliveryDriverUser? DeliveryDriver { get; set; }

    [ForeignKey("RentalPlanId")]
    public RentalPlan? RentalPlan { get; set; }

    [Required(ErrorMessage = "StartDate is required")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "ExpectedEndDate is required")]
    [DataType(DataType.Date)]
    public DateTime ExpectedEndDate { get; set; }

    [Required(ErrorMessage = "TotalCost is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost { get; set; }

    [Required(ErrorMessage = "PenaltyCost is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PenaltyCost { get; set; }
}
