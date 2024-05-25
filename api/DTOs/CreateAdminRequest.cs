namespace Motto.Models
{
    public class CreateAdminRequest
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
