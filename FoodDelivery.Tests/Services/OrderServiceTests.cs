using Autofac;
using FoodDelivery.BLL.Services.Interfaces;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.UoW;
using FoodDelivery.Tests.Fixtures;
using NSubstitute;

namespace FoodDelivery.Tests.Services
{
    public class OrderServiceTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _mockUnitOfWork;

        public OrderServiceTests(TestFixture fixture)
        {
            _fixture = fixture;
            _orderService = fixture.Container.Resolve<IOrderService>();
            _mockUnitOfWork = fixture.Container.Resolve<IUnitOfWork>();
        }

        [Fact]
        public void CreateOrder_ShouldReturnNewOrderWithCorrectStatus()
        {//має повернути нове замовлення з правильним статусом

            // Arrange
            _mockUnitOfWork.Orders.When(x => x.Add(Arg.Any<Order>())).Do(info =>
               {
                   var order = info.Arg<Order>();
                   order.Id = 1;
               });

            // Act
            var result = _orderService.CreateOrder();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Нове", result.OrderStatus);
            _mockUnitOfWork.Received(1).Save();  // перевірка що Save() був викликаний один раз
        }

        [Fact]
        public void AddDishToOrder_ShouldAddDishToExistingOrder()
        {// має додати страву до існуйочого замовлення

            // Arrange
            var orderId = 1;
            var dishId = 5;
            var quantity = 2;

            var order = new Order { Id = orderId, OrderStatus = "Нове", OrderDate = DateTime.Now };
            var dish = new Dish { Id = dishId, Name = "Test Dish", Price = 100.0M };

            // налаштування заглушки (для імітування запитів LINQ до БД)
            var orderQueryable = Substitute.For<IQueryable<Order>>();
            var orderEnumerable = new List<Order> { order }.AsQueryable();
            orderQueryable.Provider.Returns(orderEnumerable.Provider);
            orderQueryable.Expression.Returns(orderEnumerable.Expression);
            orderQueryable.ElementType.Returns(orderEnumerable.ElementType);
            orderQueryable.GetEnumerator().Returns(orderEnumerable.GetEnumerator());

            var dishQueryable = Substitute.For<IQueryable<Dish>>();
            var dishEnumerable = new List<Dish> { dish }.AsQueryable();
            dishQueryable.Provider.Returns(dishEnumerable.Provider);
            dishQueryable.Expression.Returns(dishEnumerable.Expression);
            dishQueryable.ElementType.Returns(dishEnumerable.ElementType);
            dishQueryable.GetEnumerator().Returns(dishEnumerable.GetEnumerator());

            // налаштовуєио заглушку UoW,
            // коли викликається GetById(), повертаємо наші заглушки IQueryable
            _mockUnitOfWork.Orders.GetById(orderId).Returns(orderQueryable);
            _mockUnitOfWork.Dishes.GetById(dishId).Returns(dishQueryable);

            // Act
            _orderService.AddDishToOrder(orderId, dishId, quantity);

            // Assert
            _mockUnitOfWork.Orders.Received(1).AddDishToOrder(
                Arg.Is<Order>(o => o.Id == orderId),
                Arg.Is<Dish>(d => d.Id == dishId),
                Arg.Is<int>(q => q == quantity));
            _mockUnitOfWork.Received(2).Save();
        }

        [Fact]
        public void GetOrderItems_ShouldReturnOrderItemsForExistingOrder()
        {// має повертати елементи замовлення для існуючого замовлення

            // Arrange
            var orderId = 1;
            var dish1 = new Dish { Id = 1, Name = "Dish 1", Price = 100.0M };
            var dish2 = new Dish { Id = 2, Name = "Dish 2", Price = 150.0M };

            var orderItems = new List<OrderItem>
            {
                new OrderItem { Id = 1, OrderId = orderId, DishId = 1, Dish = dish1, Quantity = 2, Price = 100.0M },
                new OrderItem { Id = 2, OrderId = orderId, DishId = 2, Dish = dish2, Quantity = 1, Price = 150.0M }
            };

            _mockUnitOfWork.Orders.GetOrderItems(orderId).Returns(orderItems);

            // Act
            var result = _orderService.GetOrderItems(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(100.0M, result[0].Price);
            Assert.Equal(150.0M, result[1].Price);
            Assert.Equal(2, result[0].Quantity);
            Assert.Equal(1, result[1].Quantity);
        }

        [Fact]
        public void CalculateTotalOrderPrice_ShouldReturnCorrectTotal()
        {// має повертати корекстну суму

            // Arrange
            var orderId = 1;
            var expectedTotal = 350.0M;

            _mockUnitOfWork.Orders.CalculateTotalPrice(orderId).Returns(expectedTotal);

            // Act
            var result = _orderService.CalculateTotalOrderPrice(orderId);

            // Assert
            Assert.Equal(expectedTotal, result);
        }
    }
}