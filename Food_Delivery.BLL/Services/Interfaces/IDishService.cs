using FoodDelivery.BLL.Models;

namespace FoodDelivery.BLL.Services.Interfaces
{
    public interface IDishService
    {
        List<DishDto> GetAllDishes();
        DishDto GetDishById(int id);
        List<DishDto> SearchDishesByName(string name);
    }
}
