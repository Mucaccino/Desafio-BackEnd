namespace Motto.Models;

public class DeliveryDriver : User
{
    public string CNPJ { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string DriverLicenseNumber { get; set; }
    public string DriverLicenseType { get; set; }
    public string DriverLicenseImage { get; set; } // Path of license image
}