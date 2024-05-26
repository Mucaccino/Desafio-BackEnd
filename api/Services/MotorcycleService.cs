using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Controllers;
using Motto.Entities;
using Motto.Models;
using Motto.Services.EventProducers;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;

namespace Motto.Services
{

    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<ServiceResult<string>> CreateMotorcycle(CreateMotorcycleRequest model, MotorcycleEventProducer? motorcycleEventProducer)
        {
            var existingPlateMotorcycle = await _motorcycleRepository.GetByPlate(model.Plate);
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
                await _motorcycleRepository.Add(motorcycle);
                await _motorcycleRepository.SaveChanges();

                motorcycleEventProducer?.PublishMotorcycleRegisteredEvent(motorcycle);

                return ServiceResult<string>.Successed("Moto cadastrada com sucesso");
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message?.Contains("Plate") ?? false)
            {
                return ServiceResult<string>.Failed("Já existe uma moto com essa placa");
            }
        }

        public async Task<ServiceResult<string>> UpdateMotorcycle(int id, CreateMotorcycleRequest model)
        {
            var existingMotorcycle = await _motorcycleRepository.GetById(id);

            if (existingMotorcycle == null)
            {
                return ServiceResult<string>.Failed("Moto não encontrada");
            }

            var existingPlateMotorcycle = await _motorcycleRepository.HasPlate(model.Plate, id);
            if (existingPlateMotorcycle)
            {
                return ServiceResult<string>.Failed("Já existe uma moto com essa placa");
            }

            existingMotorcycle.Year = model.Year;
            existingMotorcycle.Model = model.Model;
            existingMotorcycle.Plate = model.Plate;

            try
            {
                await _motorcycleRepository.Update(existingMotorcycle.Id);
                await _motorcycleRepository.SaveChanges();

                return ServiceResult<string>.Successed("Moto atualizada com sucesso");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<string>.Failed("Erro ao atualizar a moto: " + ex.Message);
            }
        }

        public async Task<Motorcycle?> GetMotorcycleById(int id)
        {
            return await _motorcycleRepository.GetById(id);
        }

        public async Task<IEnumerable<Motorcycle>> GetMotorcycles(string? plateFilter)
        {
            return await _motorcycleRepository.GetAll(plateFilter);
        }

        public async Task<ServiceResult<string>> RemoveMotorcycle(int id)
        {
            var motorcycle = await _motorcycleRepository.GetById(id);

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
                await _motorcycleRepository.Remove(motorcycle.Id);
                await _motorcycleRepository.SaveChanges();

                return ServiceResult<string>.Successed("Moto removida com sucesso.");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<string>.Failed("Erro ao remover a moto: " + ex.Message);
            }
        }
    }
}
