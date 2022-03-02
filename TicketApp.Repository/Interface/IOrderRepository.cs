using System;
using System.Collections.Generic;
using System.Text;
using TicketApp.Domain.DomainModels;

namespace TicketApp.Repository.Interface
{
    public interface IOrderRepository
    {
        List<Order> getAllOrders();
        Order getOrderDetails(BaseEntity model);
    }
}
