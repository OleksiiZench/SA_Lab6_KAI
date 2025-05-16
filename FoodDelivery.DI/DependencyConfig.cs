using Autofac;
using AutoMapper;
using FoodDelivery.BLL.Mapping;
using FoodDelivery.BLL.Services;
using FoodDelivery.BLL.Services.Interfaces;
using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Repositories;
using FoodDelivery.DAL.Repositories.Interfaces;
using FoodDelivery.DAL.UoW;

namespace FoodDelivery.DI
{
    public static class DependencyConfig
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // Регіструємо контекст
            builder.RegisterType<AppDbContext>().AsSelf().InstancePerLifetimeScope();

            // Регіструємо репозиторії
            builder.RegisterType<DishRepository>().As<IDishRepository>();
            builder.RegisterType<MenuRepository>().As<IMenuRepository>();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>();

            // Регіструємо UoW
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            // Регіструємо сервіси
            builder.RegisterType<DishService>().As<IDishService>();
            builder.RegisterType<MenuService>().As<IMenuService>();
            builder.RegisterType<OrderService>().As<IOrderService>();

            // Регіструємо AutoMapper
            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            })).AsSelf().SingleInstance();

            builder.Register(ctx =>
            {
                var mapper = ctx.Resolve<MapperConfiguration>().CreateMapper();
                return mapper;
            }).As<IMapper>().InstancePerLifetimeScope();
        }
    }
}
