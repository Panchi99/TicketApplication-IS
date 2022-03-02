using System;
using System.Collections.Generic;
using System.Text;
using TicketApp.Domain.DomainModels;
using TicketApp.Domain.DTO;

namespace TicketApp.Services.Interface
{
        public interface ITicketService
        {
            List<Ticket> GetAllTickets();
            Ticket GetDetailsForTicket(Guid? id);
            void CreateNewTicket(Ticket p);
            void UpdeteExistingTicket(Ticket p);
            AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
            void DeleteTicket(Guid id);
            bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
        }
    }

