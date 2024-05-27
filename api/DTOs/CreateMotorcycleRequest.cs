namespace Motto.DTOs
{
    /// <summary>
    /// Represents a request to create a motorcycle.
    /// </summary>
    public class CreateMotorcycleRequest
    {
        /// <summary>
        /// Gets or sets the year of the motorcycle.
        /// </summary>
        public required int Year { get; set; }

        /// <summary>
        /// Gets or sets the model of the motorcycle.
        /// </summary>
        public required string Model { get; set; }

        /// <summary>
        /// Gets or sets the plate of the motorcycle.
        /// </summary>
        public required string Plate { get; set; }
    }
}
