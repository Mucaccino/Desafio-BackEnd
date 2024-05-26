namespace Motto.Models
{
    public class CreateDeliveryDriverRequest
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string CNPJ { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string DriverLicenseNumber { get; set; }
        public required string DriverLicenseType { get; set; }
        public string? DriverLicenseImage { get; set; }
    }
}
