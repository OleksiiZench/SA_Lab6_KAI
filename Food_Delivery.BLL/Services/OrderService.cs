using AutoMapper;
using FoodDelivery.BLL.Models;
using FoodDelivery.BLL.Services.Interfaces;
using FoodDelivery.DAL.Entities;
using FoodDelivery.DAL.UoW;

namespace FoodDelivery.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public OrderDto CreateOrder()
        {
            var newOrder = new Order { OrderDate = DateTime.Now, OrderStatus = "Нове" };
            _unitOfWork.Orders.Add(newOrder);
            _unitOfWork.Save();
            return _mapper.Map<OrderDto>(newOrder);
        }

        public void AddDishToOrder(int orderId, int dishId, int quantity)
        {
            var order = _unitOfWork.Orders.GetById(orderId).SingleOrDefault();
            var dish = _unitOfWork.Dishes.GetById(dishId).SingleOrDefault();

            if (order != null && dish != null)
            {
                _unitOfWork.Orders.AddDishToOrder(order, dish, quantity);
                _unitOfWork.Save();
            }
        }

        public List<OrderItemDto> GetOrderItems(int orderId)
        {
            var orderItems = _unitOfWork.Orders.GetOrderItems(orderId);
            return _mapper.Map<List<OrderItemDto>>(orderItems);
        }

        public decimal CalculateTotalOrderPrice(int orderId)
        {
            return _unitOfWork.Orders.CalculateTotalPrice(orderId);
        }

        public bool RemoveOrderItem(int orderId, int itemId)
        {
            var orderItem = _unitOfWork.Orders.GetOrderItems(orderId)
                .FirstOrDefault(oi => oi.Id == itemId);

            if (orderItem == null)
                return false;

            _unitOfWork.Orders.RemoveOrderItem(orderItem);
            _unitOfWork.Save();
            return true;
        }

        public bool UpdateOrderItem(int orderId, int itemId, int quantity)
        {
            if (quantity <= 0)
                return false;

            var orderItem = _unitOfWork.Orders.GetOrderItems(orderId)
                .FirstOrDefault(oi => oi.Id == itemId);

            if (orderItem == null)
                return false;

            _unitOfWork.Orders.UpdateOrderItemQuantity(itemId, quantity);
            _unitOfWork.Save();
            return true;
        }
    }
}
