namespace Motto.DTOs
{
    /// <summary>
    /// Represents a request to create a rental.
    /// </summary>
    public class RentalCreateRequest
    {
        /// <summary>
        /// Gets or sets the ID of the motorcycle to be rented.
        /// </summary>
        public int MotorcycleId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the rental plan.
        /// </summary>
        public int RentalPlanId { get; set; }
    }
}
