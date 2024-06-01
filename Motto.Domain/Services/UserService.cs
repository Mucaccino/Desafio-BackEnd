using Microsoft.EntityFrameworkCore;
using Motto.Domain.Services.Results;
using OneOf;
using Motto.Data.Repositories.Interfaces;
using Motto.Data.Enums;
using Motto.Data.Entities;
using Motto.Domain.Services.Interfaces;

namespace Motto.Domain.Services
{
    /// <summary>
    /// Represents a service for managing user operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryDriverUserRepository _deliveryDriverUserRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserService(IUserRepository userRepository, IDeliveryDriverUserRepository deliveryDriverUserRepository)
        {
            _userRepository = userRepository;
            _deliveryDriverUserRepository = deliveryDriverUserRepository;
        }

        public async Task<ServiceResult<List<User>>> GetAllUsers(UserType? type = null, string? filter = null)
        {
            var result = await _userRepository.GetAll(type, filter);
            return ServiceResult<List<User>>.Successed(result);
        }
        public async Task<ServiceResult<List<DeliveryDriverUser>>> GetAllDeliveryDriverUsers(string? filter = null)
        {
            var result = await _deliveryDriverUserRepository.GetAll(filter);
            return ServiceResult<List<DeliveryDriverUser>>.Successed(result);
        }

        /// <summary>
        /// Registers an admin user.
        /// </summary>
        /// <param name="registerModel">The registration model.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<ServiceResult<string>> RegisterAdmin(AdminUser adminUser)
        {
            try
            {
                await _userRepository.Add(adminUser);
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
        public async Task<ServiceResult<string>> RegisterDeliveryDriver(DeliveryDriverUser deliveryDriverUser)
        {
            try
            {
                await _userRepository.Add(deliveryDriverUser);
                return ServiceResult<string>.Successed("Entregador criado com sucesso");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResult<string>.Failed("Erro ao salvar os dados no banco de dados: " + ex.Message);
            }
        }
    }
}
