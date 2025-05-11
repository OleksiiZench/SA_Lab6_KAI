using System.Security.Cryptography.X509Certificates;
using Autofac;
using AutoMapper;
using FoodDelivery.BLL.Mapping;
using FoodDelivery.BLL.Services;
using FoodDelivery.BLL.Services.Interfaces;
using FoodDelivery.DAL.UoW;
using NSubstitute;


namespace FoodDelivery.Tests.Configuration
{
    public static class TestDependencyConfig
    {
        public static ContainerBuilder ConfigureTestContainer()
        {
            var builder = new ContainerBuilder();

            // Створюємо mocks використовуючи NSubstitute
            var mockUnitOfWork = Substitute.For<IUnitOfWork>();

            // Реєструємо moked UoW
            builder.RegisterInstance(mockUnitOfWork).As<IUnitOfWork>();

            // Реєструємо AutoMapper
            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            })).AsSelf().SingleInstance();

            builder.Register(ctx =>
            {
                var mapper = ctx.Resolve<MapperConfiguration>().CreateMapper();
                return mapper;
            }).As<IMapper>().InstancePerLifetimeScope();

            // Реєструємо services
            builder.RegisterType<DishService>().As<IDishService>();
            builder.RegisterType<MenuService>().As<IMenuService>();
            builder.RegisterType<OrderService>().As<IOrderService>();

            return builder;
        }
    }
}
