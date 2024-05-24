using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Motto.Controllers;
using Motto.Entities;
using Motto.Models;
using Motto.Repositories;

namespace Motto.Services
{
    public interface IUserService
    {
        Task<ServiceResult<string>> RegisterAdmin(RegisterAdminModel registerModel);
        Task<ServiceResult<string>> RegisterDeliveryDriver(RegisterDeliveryDriverModel registerModel);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ServiceResult<string>> RegisterAdmin(RegisterAdminModel registerModel)
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
                await _userRepository.AddUserAsync(user);
                await _userRepository.SaveChangesAsync();
                return ServiceResult<string>.Successed("Administrador criado com sucesso");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<string>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }

        public async Task<ServiceResult<string>> RegisterDeliveryDriver(RegisterDeliveryDriverModel registerModel)
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
                await _userRepository.AddUserAsync(user);
                await _userRepository.SaveChangesAsync();
                return ServiceResult<string>.Successed("Entregador criado com sucesso");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<string>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }
    }
}
