namespace Motto.Models;

public class Rental
{
    public int Id { get; set; }    
    public int MotorcycleId { get; set; } // Chave estrangeira para Motorcycle
    public int DeliveryDriverId { get; set; } // Chave estrangeira para DeliveryDriver
 
    public Motorcycle Motorcycle { get; set; }
    public DeliveryDriver DeliveryDriver { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public decimal TotalCost { get; set; }
    public decimal PenaltyCost { get; set; }
}
