using System;
using System.Collections.Generic;
using System.Text;
using TicketApp.Domain.DomainModels;
using TicketApp.Repository.Interface;
using TicketApp.Services.Interface;

namespace TicketApp.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public List<Order> getAllOrders()
        {
            return this._orderRepository.getAllOrders();
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return this._orderRepository.getOrderDetails(model);
        }
    }
}
