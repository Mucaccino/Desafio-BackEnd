namespace Motto.Models
{
    public class RentalDeliveryResponse
    {
        public required TotalCostModel Cost { get; set; }
        public required string Message { get; set; }
    }
}
