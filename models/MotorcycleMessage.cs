namespace Motto.Models;

public class MotorcycleMessage
{
    public required int Id { get; set; }
    public required string Content { get; set; }
    public DateTime ReceivedDate { get; set; }
}