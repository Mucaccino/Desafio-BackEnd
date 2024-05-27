using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Controllers;
using Motto.Entities;
using Motto.DTOs;
using Motto.Services.EventProducers;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;
using Motto.Models;

namespace Motto.Services
{
    /// <summary>
    /// A service for managing motorcycles.
    /// </summary>
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MotorcycleService"/> class.
        /// </summary>
        /// <param name="motorcycleRepository">The repository for motorcycles.</param>
        public MotorcycleService(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        /// <summary>
        /// Creates a new motorcycle with the given request model and sends a motorcycle registered event.
        /// </summary>
        /// <param name="model">The request model containing the motorcycle's information.</param>
        /// <param name="motorcycleEventProducer">The motorcycle event producer used to send the registered event. Can be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ServiceResult with a string message. If the motorcycle already exists, the result will be a failed ServiceResult with an error message. Otherwise, it will be a successful ServiceResult with a success message.</returns>
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

        /// <summary>
        /// Updates a motorcycle with the given ID using the provided request model.
        /// </summary>
        /// <param name="id">The ID of the motorcycle to update.</param>
        /// <param name="model">The request model containing the updated motorcycle information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ServiceResult with a string message. If the motorcycle is not found, the result will be a failed ServiceResult with an error message. If a motorcycle with the same plate already exists, the result will be a failed ServiceResult with an error message. Otherwise, the motorcycle will be updated and the result will be a successful ServiceResult with a success message.</returns>
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

        /// <summary>
        /// Retrieves a motorcycle by its ID.
        /// </summary>
        /// <param name="id">The ID of the motorcycle.</param>
        /// <returns>An asynchronous task that returns a Motorcycle object representing the motorcycle with the specified ID, or null if no motorcycle is found.</returns>
        public async Task<Motorcycle?> GetMotorcycleById(int id)
        {
            return await _motorcycleRepository.GetById(id);
        }

        /// <summary>
        /// Retrieves a list of motorcycles filtered by plate.
        /// </summary>
        /// <param name="plateFilter">The plate filter to apply to the motorcycles. Optional.</param>
        /// <returns>An asynchronous task that returns an IEnumerable of Motorcycle objects.</returns>
        public async Task<IEnumerable<Motorcycle>> GetMotorcycles(string? plateFilter)
        {
            return await _motorcycleRepository.GetAll(plateFilter);
        }

        /// <summary>
        /// Removes a motorcycle with the given ID.
        /// </summary>
        /// <param name="id">The ID of the motorcycle to remove.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ServiceResult with a string message. If the motorcycle is not found, the result will be a failed ServiceResult with an error message. If the motorcycle has rentals associated with it, the result will be a failed ServiceResult with an error message. Otherwise, the motorcycle will be removed and the result will be a successful ServiceResult with a success message.</returns>
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
