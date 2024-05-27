namespace Motto.DTOs
{
    public class CreateAdminRequest
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
