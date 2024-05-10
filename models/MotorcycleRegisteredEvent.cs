namespace Motto.Models;

public class MotorcycleRegisteredEvent
{
    public int Id { get; set; }
    public Motorcycle Motorcycle { get; set; }
    public DateTime EventDate { get; set; }
}