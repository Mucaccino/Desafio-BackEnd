
using AutoMapper;
using Motto.Data.Entities;
using Motto.WebApi.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Motto.WebApi;

/// <summary>
/// AutoMapper profile for mapping between DTOs and entities.
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Creates a new instance of the AutoMapperProfile class.
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<UserCreateRequest, User>();
        CreateMap<UserCreateRequest, AdminUser>();
        CreateMap<DeliveryDriverCreateRequest, DeliveryDriverUser>();
        CreateMap<RentalCreateRequest, Rental>();
        CreateMap<MotorcycleCreateRequest, Motorcycle>();
    }
}