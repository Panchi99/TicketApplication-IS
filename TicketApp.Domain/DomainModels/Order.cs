using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TicketApp.Domain.Identity;
using TicketApp.Domain.DTO;

namespace TicketApp.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        
        public string UserId { get; set; }
        public TicketAppApplicationUser User { get; set; }

        public IEnumerable<TicketInOrder> TicketInOrder { get; set; }

    }
}
