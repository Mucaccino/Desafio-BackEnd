namespace Motto.DTOs
{
    public class CreateMotorcycleRequest
    {
        public required int Year { get; set; }
        public required string Model { get; set; }
        public required string Plate { get; set; }
    }
}
