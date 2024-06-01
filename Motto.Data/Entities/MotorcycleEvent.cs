using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Motto.Data.Entities;

/// <summary>
/// MotorcycleEvent class
/// </summary>
[Table("MotorcycleEvents")]
public class MotorcycleEvent
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Motorcycle is required")]
    public required Motorcycle Motorcycle { get; set; }

    [Required(ErrorMessage = "EventDate is required")]
    [DataType(DataType.Date)]
    public DateTime EventDate { get; set; }
}