using System.ComponentModel.DataAnnotations;

namespace Motto.Dtos
{
    /// <summary>
    /// Login request
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required(ErrorMessage = "O campo 'Username' é obrigatório.")]
        public required string Username { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        /// <value>
        /// The password must be between 8 and 20 characters, contain at least one letter and one number
        /// </value>
        [Required(ErrorMessage = "O campo 'Password' é obrigatório.")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,20}$", 
            ErrorMessage = "A senha deve ter no mínimo 8 e no máximo 20 caracteres, contendo pelo menos uma letra e um número.")]
        public required string Password { get; set; }
    }
}
