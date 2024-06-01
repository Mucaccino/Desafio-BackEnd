using Motto.Domain.Models;

namespace Motto.Domain.Services.Results
{
    /// <summary>
    /// Represents a response containing the total cost of a rental.
    /// </summary>
    public class RentalDeliverResult
    {
        /// <summary>
        /// Gets or sets the total cost of the rental.
        /// </summary>
        public required TotalCostModel Cost { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public required string Message { get; set; }
    }
}
