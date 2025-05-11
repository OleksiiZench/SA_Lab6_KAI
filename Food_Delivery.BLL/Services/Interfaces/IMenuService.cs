using FoodDelivery.BLL.Models;

namespace FoodDelivery.BLL.Services.Interfaces
{
    public interface IMenuService
    {
        List<DishDto> GetMenuForDay(int dayOfWeekId);
        List<DishDto> GetDishesByCategory(int categoryId);
    }
}
