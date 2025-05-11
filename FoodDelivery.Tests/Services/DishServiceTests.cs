using Autofac;
using FoodDelivery.BLL.Services.Interfaces;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.UoW;
using FoodDelivery.Tests.Fixtures;
using NSubstitute;

namespace FoodDelivery.Tests.Services
{
    public class DishServiceTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly IDishService _dishService;
        private readonly IUnitOfWork _mockUnitOfWork;

        public DishServiceTests(TestFixture fixture)
        {
            _fixture = fixture;
            _dishService = fixture.Container.Resolve<IDishService>();
            _mockUnitOfWork = fixture.Container.Resolve<IUnitOfWork>();
        }

        [Fact]
        public void GetAllDishes_ShouldReturnAllDishes()
        {// має повернути всі страви

            // Arrange
            var dishes = new List<Dish>
            {
                new Dish { Id = 1, Name = "Dish 1", Price = 100.0M },
                new Dish { Id = 2, Name = "Dish 2", Price = 150.0M },
                new Dish { Id = 3, Name = "Dish 3", Price = 200.0M }
            };

            _mockUnitOfWork.Dishes.GetAll().Returns(dishes);

            // Act
            var result = _dishService.GetAllDishes();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Dish 1", result[0].Name);
            Assert.Equal("Dish 2", result[1].Name);
            Assert.Equal("Dish 3", result[2].Name);
        }

        [Fact]
        public void GetDishById_ShouldReturnCorrectDish()
        {// має повернути коректну страву

            // Arrange
            var dishId = 1;
            var dish = new Dish { Id = dishId, Name = "Test Dish", Price = 100.0M };

            var dishQueryable = Substitute.For<IQueryable<Dish>>();
            var dishEnumerable = new List<Dish> { dish }.AsQueryable();
            dishQueryable.Provider.Returns(dishEnumerable.Provider);
            dishQueryable.Expression.Returns(dishEnumerable.Expression);
            dishQueryable.ElementType.Returns(dishEnumerable.ElementType);
            dishQueryable.GetEnumerator().Returns(dishEnumerable.GetEnumerator());

            _mockUnitOfWork.Dishes.GetById(dishId).Returns(dishQueryable);

            // Act
            var result = _dishService.GetDishById(dishId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dishId, result.Id);
            Assert.Equal("Test Dish", result.Name);
            Assert.Equal(100.0M, result.Price);
        }

        [Fact]
        public void GetDishById_ShouldReturnNull_WhenDishNotFound()
        {// має повернути null, якщо страву не знайдено

            // Arrange
            var dishId = 999;
            var dishQueryable = Substitute.For<IQueryable<Dish>>();
            var dishEnumerable = new List<Dish>().AsQueryable();
            dishQueryable.Provider.Returns(dishEnumerable.Provider);
            dishQueryable.Expression.Returns(dishEnumerable.Expression);
            dishQueryable.ElementType.Returns(dishEnumerable.ElementType);
            dishQueryable.GetEnumerator().Returns(dishEnumerable.GetEnumerator());

            _mockUnitOfWork.Dishes.GetById(dishId).Returns(dishQueryable);

            // Act
            var result = _dishService.GetDishById(dishId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SearchDishesByName_ShouldReturnMatchingDishes()
        {// має повернутися відповідна страва за назвою

            // Arrange
            var searchTerm = "pasta";
            var dishes = new List<Dish>
            {
                new Dish { Id = 1, Name = "Pasta Carbonara", Price = 150.0M },
                new Dish { Id = 2, Name = "Pasta Bolognese", Price = 180.0M }
            };

            _mockUnitOfWork.Dishes.SearchByName(searchTerm).Returns(dishes);

            // Act
            var result = _dishService.SearchDishesByName(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Pasta Carbonara", result[0].Name);
            Assert.Equal("Pasta Bolognese", result[1].Name);
        }

        [Fact]
        public void SearchDishesByName_ShouldReturnEmptyList_WhenNoMatches()
        {// має повертати порожній List, коли немає збігів за назвою

            // Arrange
            var searchTerm = "nonexistent";
            _mockUnitOfWork.Dishes.SearchByName(searchTerm).Returns(new List<Dish>());

            // Act
            var result = _dishService.SearchDishesByName(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
