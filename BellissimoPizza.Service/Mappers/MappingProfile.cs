using AutoMapper;
using BellisimoPizza.Domain.Entities.Orders;
using BellisimoPizza.Domain.Entities.Pizzas;
using BellisimoPizza.Domain.Entities.Users;
using BellisimoPizza.Service.DTOs.Orders;
using BellisimoPizza.Service.DTOs.Pizzas;
using BellisimoPizza.Service.DTOs.Users;

namespace BellisimoPizza.Service.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PizzaForCreationDto, Pizza>().ReverseMap();
            CreateMap<OrderForCreationDto, Order>().ReverseMap();
            CreateMap<UserForCreationDto, User>().ReverseMap();

        }
    }
}
