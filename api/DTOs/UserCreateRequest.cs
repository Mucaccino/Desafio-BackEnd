using System.ComponentModel.DataAnnotations;

namespace Motto.DTOs
{
    /// <summary>
    /// Represents a request to create an user.
    /// </summary>
    public class UserCreateRequest
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 8)]
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        /// <value>The password must be between 8 and 20 characters, contain at least one letter and one number</value>
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,20}$",
            ErrorMessage = "A senha deve ter no mínimo 8 e no máximo 20 caracteres, contendo pelo menos uma letra e um número.")]
        public required string Password { get; set; }
    }
}
