namespace Motto.Models
{
    public class CreateMotorcycleRequest
    {
        public required int Year { get; set; }
        public required string Model { get; set; }
        public required string Plate { get; set; }
    }
}
