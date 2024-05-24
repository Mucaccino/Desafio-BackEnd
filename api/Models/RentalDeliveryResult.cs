namespace Motto.Models
{
    public class RentalDeliveryResult
    {
        public required RentalTotalCostModel Cost { get; set; }
        public required string Message { get; set; }
    }
}
