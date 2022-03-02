using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TicketApp.Domain.DomainModels;

namespace TicketApp.Domain.DTO
{
    public class TicketInShoppingCart : BaseEntity
    {
        public Guid  TicketId{ get; set; }
        public Ticket Ticket { get; set; }
        public Guid ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}
