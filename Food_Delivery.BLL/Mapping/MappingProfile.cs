using AutoMapper;
using FoodDelivery.BLL.Models;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Dish, DishDto>().ReverseMap();
            CreateMap<DAL.Entities.DayOfWeek, DayOfWeekDto>().ReverseMap();
            CreateMap<Menu, MenuDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        }
    }
}
