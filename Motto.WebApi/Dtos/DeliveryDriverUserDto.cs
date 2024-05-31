public class DeliveryDriverUserDto
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Name { get; set; } = string.Empty;

    public string CNPJ { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string DriverLicenseNumber { get; set; } = string.Empty;

    public string DriverLicenseType { get; set; } = string.Empty;

    public string? DriverLicenseImage { get; set; }

}