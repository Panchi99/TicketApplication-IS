using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TicketApp.Domain.DomainModels;
using TicketApp.Repository.Interface;

namespace TicketApp.Repository.Implementation
{
        public class OrderRepository : IOrderRepository
        {

            private readonly ApplicationDbContext context;
            private DbSet<Order> entities;
            string errorMessage = string.Empty;

            public OrderRepository(ApplicationDbContext context)
            {
                this.context = context;
                entities = context.Set<Order>();
            }


            public List<Order> getAllOrders()
            {
                return entities
                    .Include(z => z.User)
                    .Include(z => z.TicketInOrder)
                    .Include("TicketInOrder.OrderedTicket")
                    .ToListAsync().Result;
            }

            public Order getOrderDetails(BaseEntity model)
            {
                return entities
                   .Include(z => z.User)
                   .Include(z => z.TicketInOrder)
                   .Include("TicketInOrder.OrderedTicket")
                   .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
            }
        }
    
}
