using System;
using System.Collections.Generic;
using System.Text;
using TicketApp.Domain.DomainModels;

namespace TicketApp.Services.Interface
{
    public interface IOrderService
    {
        List<Order> getAllOrders();
        Order getOrderDetails(BaseEntity model);
    }
}
