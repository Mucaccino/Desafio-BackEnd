using System.ComponentModel.DataAnnotations;

namespace Motto.DTOs
{
    /// <summary>
    /// Represents a request to create a motorcycle.
    /// </summary>
    public class MotorcycleCreateRequest
    {
        /// <summary>
        /// Gets or sets the year of the motorcycle.
        /// </summary>
        [Required(ErrorMessage = "O ano da motocicleta é obrigatório.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "O ano da motocicleta deve estar no formato YYYY (quatro dígitos).")]
        public required int Year { get; set; }

        /// <summary>
        /// Gets or sets the model of the motorcycle.
        /// </summary>
        [Required(ErrorMessage = "O modelo da motocicleta é obrigatório.")]
        public required string Model { get; set; }

        /// <summary>
        /// Gets or sets the plate of the motorcycle.
        /// </summary>
        [Required(ErrorMessage = "A placa da motocicleta é obrigatória.")]
        [RegularExpression(@"^[A-Za-z]{3}-\d{4}$", ErrorMessage = "O número da placa da motocicleta deve estar no formato AAA-9999.")]
        public required string Plate { get; set; }
    }
}
