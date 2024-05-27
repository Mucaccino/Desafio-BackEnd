namespace Motto.DTOs
{
    /// <summary>
    /// Represents a request to create a delivery driver.
    /// </summary>
    public class CreateDeliveryDriverRequest
    {
        /// <summary>
        /// Gets or sets the name of the delivery driver.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the username of the delivery driver.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the delivery driver.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the CNPJ of the delivery driver.
        /// </summary>
        public required string CNPJ { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the delivery driver.
        /// </summary>
        public required DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the driver license number of the delivery driver.
        /// </summary>
        public required string DriverLicenseNumber { get; set; }

        /// <summary>
        /// Gets or sets the driver license type of the delivery driver.
        /// </summary>
        public required string DriverLicenseType { get; set; }

        /// <summary>
        /// Gets or sets the driver license image of the delivery driver.
        /// </summary>
        public string? DriverLicenseImage { get; set; }
    }
}
