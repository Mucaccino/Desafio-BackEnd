using Microsoft.EntityFrameworkCore;
using Motto.Entities;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;
using Motto.Domain.Services.Results;

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
