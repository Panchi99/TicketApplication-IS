using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketApp.Domain.DomainModels;

namespace TicketApp.Domain.Identity
{
    public class TicketAppApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public virtual ShoppingCart UserCart { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
