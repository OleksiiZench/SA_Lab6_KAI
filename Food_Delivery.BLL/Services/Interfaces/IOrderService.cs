using System.Xml.Serialization;
using FoodDelivery.BLL.Models;

namespace FoodDelivery.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        OrderDto CreateOrder();
        void AddDishToOrder(int orderId, int  dishId, int quantity);
        List<OrderItemDto> GetOrderItems(int orderId);
        decimal CalculateTotalOrderPrice(int orderId);
        bool RemoveOrderItem(int orderId, int itemId);
        bool UpdateOrderItem(int orderId, int itemId, int quantity);
    }
}
