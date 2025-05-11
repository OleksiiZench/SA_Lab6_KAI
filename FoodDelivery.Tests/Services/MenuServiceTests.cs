using Autofac;
using AutoFixture;
using FoodDelivery.BLL.Models;
using FoodDelivery.BLL.Services.Interfaces;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.UoW;
using FoodDelivery.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace FoodDelivery.Tests.Services
{
    public class MenuServiceTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly IMenuService _menuService;
        private readonly IUnitOfWork _mockUnitOfWork;

        public MenuServiceTests(TestFixture fixture)
        {
            _fixture = fixture;
            _menuService = fixture.Container.Resolve<IMenuService>();
            _mockUnitOfWork = fixture.Container.Resolve<IUnitOfWork>();
        }

        [Fact]
        public void GetMenuForDay_ShouldReturnDishesForSpecificDay()
        {// має повернути страви за певний день

            // Arrange
            var dayOfWeekId = 1; // Понеділок
            var dishes = new List<Dish>
            {
                new Dish { Id = 1, Name = "Понеділкова страва 1", Price = 100.0M },
                new Dish { Id = 2, Name = "Понеділкова страва 2", Price = 150.0M }
            };

            _mockUnitOfWork.Menus.GetDishesByDayOfWeek(dayOfWeekId).Returns(dishes);

            // Act
            var result = _menuService.GetMenuForDay(dayOfWeekId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Понеділкова страва 1", result[0].Name);
            Assert.Equal("Понеділкова страва 2", result[1].Name);
        }

        [Fact]
        public void GetMenuForDay_ShouldReturnEmptyList_WhenNoMenuForDay()
        {
            // Arrange
            var dayOfWeekId = 8; // Неіснуючий день
            _mockUnitOfWork.Menus.GetDishesByDayOfWeek(dayOfWeekId).Returns(new List<Dish>());

            // Act
            var result = _menuService.GetMenuForDay(dayOfWeekId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetDishesByCategory_ShouldReturnDishesForSpecificCategory()
        {
            // Arrange
            var categoryId = 1; // Салати
            var dishes = new List<Dish>
            {
                new Dish { Id = 1, Name = "Салат Цезар", Price = 120.0M, CategoryId = categoryId },
                new Dish { Id = 2, Name = "Грецький салат", Price = 110.5M, CategoryId = categoryId }
            };

            _mockUnitOfWork.Dishes.GetByCategoryId(categoryId).Returns(dishes);

            // Act
            var result = _menuService.GetDishesByCategory(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Салат Цезар", result[0].Name);
            Assert.Equal("Грецький салат", result[1].Name);
        }

        [Fact]
        public void GetDishesByCategory_ShouldReturnEmptyList_WhenNoDishesInCategory()
        {
            // Arrange
            var categoryId = 999; // Неіснуюча категорія
            _mockUnitOfWork.Dishes.GetByCategoryId(categoryId).Returns(new List<Dish>());

            // Act
            var result = _menuService.GetDishesByCategory(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
