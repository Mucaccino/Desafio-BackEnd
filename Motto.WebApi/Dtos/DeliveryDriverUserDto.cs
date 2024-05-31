using Minio.DataModel.Notification;
using Motto.Entities;
using Motto.Enums;

public class DeliveryDriverUserDto
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Name { get; set; } = string.Empty;

    public string CNPJ { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string LicenseNumber { get; set; } = string.Empty;

    public string LicenseType { get; set; } = string.Empty;

    public string? LicenseImage { get; set; }

    public static implicit operator DeliveryDriverUserDto(DeliveryDriverUser o)
    {
        return new DeliveryDriverUserDto
        {
            Id = o.Id,
            Username = o.Username,
            Name = o.Name,
            CNPJ = o.CNPJ,
            DateOfBirth = o.DateOfBirth,
            LicenseNumber = o.DriverLicenseNumber,
            LicenseType = o.DriverLicenseType,
            LicenseImage = o.DriverLicenseImage
        };
    }

}