namespace Motto.Events;

// Event class to be published when a new motorcycle is registered
public class MotorcycleRegisteredEvent
{
    public int MotorcycleId { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Plate { get; set; } = string.Empty;
}