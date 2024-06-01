using Motto.Data.Entities;
using Motto.Domain.Events;
using Motto.Domain.Services.Results;

namespace Motto.Domain.Services.Interfaces;

public interface IMotorcycleService
{
    Task<ServiceResult<string>> CreateMotorcycle(Motorcycle model, MotorcycleEventProducer? motorcycleEventProducer);
    Task<Motorcycle?> GetMotorcycleById(int id);
    Task<IEnumerable<Motorcycle>> GetMotorcycles(string? plateFilter);
    Task<ServiceResult<string>> RemoveMotorcycle(int id);
    Task<ServiceResult<string>> UpdateMotorcycle(int id, Motorcycle model);
}