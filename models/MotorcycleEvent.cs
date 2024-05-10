namespace Motto.Models;

public class MotorcycleEvent
{
    public int Id { get; set; }
    public Motorcycle Motorcycle { get; set; }
    public DateTime EventDate { get; set; }
}