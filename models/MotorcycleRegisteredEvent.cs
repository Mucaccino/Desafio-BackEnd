namespace Motto.Models;

// Classe de evento de moto cadastrada
public class MotorcycleRegisteredEvent
{
    public int MotorcycleId { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Plate { get; set; }
}