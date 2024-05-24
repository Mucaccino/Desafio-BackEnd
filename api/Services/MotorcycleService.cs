using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Controllers;
using Motto.Entities;
using Motto.Models;
using Motto.Repositories;
using Motto.Services.EventProducers;

namespace Motto.Services
{
    public interface IMotorcycleService
    {
        Task<ServiceResult<string>> CreateMotorcycle(MotorcycleCreateModel model, MotorcycleEventProducer? motorcycleEventProducer);
        Task<ServiceResult<string>> UpdateMotorcycle(int id, MotorcycleCreateModel model);
        Task<Motorcycle?> GetMotorcycleById(int id);
        Task<IEnumerable<Motorcycle>> GetMotorcycles(string? plateFilter);
        Task<ServiceResult<string>> RemoveMotorcycle(int id);
    }

    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<ServiceResult<string>> CreateMotorcycle(MotorcycleCreateModel model, MotorcycleEventProducer? motorcycleEventProducer)
        {
            var existingPlateMotorcycle = await _motorcycleRepository.GetMotorcycleByPlate(model.Plate);
            if (existingPlateMotorcycle != null)
            {
                return ServiceResult<string>.Failed("Já existe uma moto com essa placa");
            }

            var motorcycle = new Motorcycle
            {
                Year = model.Year,
                Model = model.Model,
                Plate = model.Plate
            };

            try
            {
                await _motorcycleRepository.AddMotorcycle(motorcycle);
                await _motorcycleRepository.SaveChangesAsync();

                motorcycleEventProducer?.PublishMotorcycleRegisteredEvent(motorcycle);

                return ServiceResult<string>.Successed("Moto cadastrada com sucesso");
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message?.Contains("Plate") ?? false)
            {
                return ServiceResult<string>.Failed("Já existe uma moto com essa placa");
            }
        }

        public async Task<ServiceResult<string>> UpdateMotorcycle(int id, MotorcycleCreateModel model)
        {
            var existingMotorcycle = await _motorcycleRepository.GetMotorcycleById(id);

            if (existingMotorcycle == null)
            {
                return ServiceResult<string>.Failed("Moto não encontrada");
            }

            var existingPlateMotorcycle = await _motorcycleRepository.GetMotorcycleByPlateAndDifferentId(model.Plate, id);
            if (existingPlateMotorcycle != null)
            {
                return ServiceResult<string>.Failed("Já existe uma moto com essa placa");
            }

            existingMotorcycle.Year = model.Year;
            existingMotorcycle.Model = model.Model;
            existingMotorcycle.Plate = model.Plate;

            try
            {
                await _motorcycleRepository.UpdateMotorcycle(existingMotorcycle.Id);
                await _motorcycleRepository.SaveChangesAsync();

                return ServiceResult<string>.Successed("Moto atualizada com sucesso");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<string>.Failed("Erro ao atualizar a moto: " + ex.Message);
            }
        }

        public async Task<Motorcycle?> GetMotorcycleById(int id)
        {
            return await _motorcycleRepository.GetMotorcycleById(id);
        }

        public async Task<IEnumerable<Motorcycle>> GetMotorcycles(string? plateFilter)
        {
            return await _motorcycleRepository.GetMotorcycles(plateFilter);
        }

        public async Task<ServiceResult<string>> RemoveMotorcycle(int id)
        {
            var motorcycle = await _motorcycleRepository.GetMotorcycleById(id);

            if (motorcycle == null)
            {
                return ServiceResult<string>.Failed("Moto não encontrada");
            }

            var hasRentals = await _motorcycleRepository.HasRentals(id);

            if (hasRentals)
            {
                return ServiceResult<string>.Failed("Não é possível remover a moto porque existem locações associadas a ela.");
            }

            try
            {
                await _motorcycleRepository.RemoveMotorcycle(motorcycle.Id);
                await _motorcycleRepository.SaveChangesAsync();

                return ServiceResult<string>.Successed("Moto removida com sucesso.");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<string>.Failed("Erro ao remover a moto: " + ex.Message);
            }
        }
    }
}
