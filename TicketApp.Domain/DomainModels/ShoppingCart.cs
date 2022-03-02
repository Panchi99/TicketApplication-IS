using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TicketApp.Domain.DTO;
using TicketApp.Domain.Identity;

namespace TicketApp.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        
        public string OwnerId { get; set; }
        public TicketAppApplicationUser Owner { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCart { get; set; }
    }
}
