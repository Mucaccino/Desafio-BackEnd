using System.ComponentModel.DataAnnotations;

namespace Motto.Dtos
{
    /// <summary>
    /// Represents a request to create a rental.
    /// </summary>
    public class RentalCreateRequest
    {
        /// <summary>
        /// Gets or sets the ID of the motorcycle to be rented.
        /// </summary>
        [Required]
        public int MotorcycleId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the rental plan.
        /// </summary>
        [Required]
        public int RentalPlanId { get; set; }
    }
}
