using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Controllers;
using Motto.Entities;
using Motto.DTOs;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;
using Motto.Models;

namespace Motto.Services
{
    /// <summary>
    /// Represents a service for managing user operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Registers an admin user.
        /// </summary>
        /// <param name="registerModel">The registration model.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<ServiceResult<string>> RegisterAdmin(CreateAdminRequest registerModel)
        {
            var user = new User
            {
                Username = registerModel.Username,
                Name = registerModel.Name,
                Type = UserType.Admin
            };

            user.SetPassword(registerModel.Password);

            try
            {
                await _userRepository.Add(user);
                await _userRepository.SaveChanges();
                return ServiceResult<string>.Successed("Administrador criado com sucesso");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<string>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }
        
        /// <summary>
        /// Registers a new delivery driver user.
        /// </summary>
        /// <param name="registerModel">The model containing the delivery driver registration details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a ServiceResult object that 
        /// contains a success message if the registration is successful, or an error message if the registration fails.</returns>
        public async Task<ServiceResult<string>> RegisterDeliveryDriver(CreateDeliveryDriverRequest registerModel)
        {
            var user = new DeliveryDriver
            {
                Username = registerModel.Username,
                Name = registerModel.Name,
                CNPJ = registerModel.CNPJ,
                DateOfBirth = registerModel.DateOfBirth,
                DriverLicenseNumber = registerModel.DriverLicenseNumber,
                DriverLicenseType = registerModel.DriverLicenseType,
                Type = UserType.DeliveryDriver
            };

            user.SetPassword(registerModel.Password);

            try
            {
                await _userRepository.Add(user);
                await _userRepository.SaveChanges();
                return ServiceResult<string>.Successed("Entregador criado com sucesso");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<string>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }
    }
}
