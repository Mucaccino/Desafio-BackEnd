using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Motto.Data.Entities;

/// <summary>
/// MotorcycleMessage class
/// </summary>
[Table("MotorcycleMessages")]
public class MotorcycleMessage
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; } = string.Empty;

    public DateTime ReceivedDate { get; set; }
}