using Motto.Models;
using Motto.Services.EventProducers;

namespace Motto.Services.Interfaces;

public interface IMotorcycleService
{
    Task<ServiceResult<string>> CreateMotorcycle(CreateMotorcycleRequest model, MotorcycleEventProducer? motorcycleEventProducer);
    Task<Motorcycle?> GetMotorcycleById(int id);
    Task<IEnumerable<Motorcycle>> GetMotorcycles(string? plateFilter);
    Task<ServiceResult<string>> RemoveMotorcycle(int id);
    Task<ServiceResult<string>> UpdateMotorcycle(int id, CreateMotorcycleRequest model);
}