namespace Motto.Dtos
{
    /// <summary>
    /// Represents a response containing the total cost of a rental.
    /// </summary>
    public class RentalDeliverResponse
    {
        /// <summary>
        /// Gets or sets the total cost of the rental.
        /// </summary>
        public required TotalCostResponse Cost { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public required string Message { get; set; }
    }
}
