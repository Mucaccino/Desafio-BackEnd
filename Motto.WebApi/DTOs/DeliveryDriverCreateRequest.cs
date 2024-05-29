using Motto.Enums;
using System.ComponentModel.DataAnnotations;

namespace Motto.DTOs
{
    /// <summary>
    /// Represents a request to create a delivery driver.
    /// </summary>
    public class DeliveryDriverCreateRequest : UserCreateRequest
    {
        /// <summary>
        /// Gets or sets the CNPJ of the delivery driver.
        /// </summary>
        [RegularExpression(@"^\d{2}\.?\d{3}\.?\d{3}/?\d{4}-?\d{2}$", ErrorMessage = "CNPJ inválido.")]
        public required string CNPJ { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the delivery driver.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2100", ErrorMessage = "Data de nascimento deve estar entre 01/01/1900 e 01/01/2100.")]
        public required DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the driver license number of the delivery driver.
        /// </summary>
        [Required(ErrorMessage = "O número da carteira de motorista é obrigatório.")]
        [RegularExpression(@"^\d{9}-?\d{2}$", ErrorMessage = "O número da carteira de motorista deve estar no formato 999999999-99 ou 99999999999.")]
        public required string DriverLicenseNumber { get; set; }

        /// <summary>
        /// Gets or sets the driver license type of the delivery driver.
        /// </summary>
        [Required(ErrorMessage = "O tipo de carteira de motorista é obrigatório.")]
        [EnumDataType(typeof(DriverLicenseType), ErrorMessage = "O tipo de carteira de motorista fornecido não é válido.")]
        public required string DriverLicenseType { get; set; }

        /// <summary>
        /// Gets or sets the driver license image of the delivery driver.
        /// </summary>
        public string? DriverLicenseImage { get; set; }
    }
}
