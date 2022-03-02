using System;
using System.Collections.Generic;
using System.Text;
using TicketApp.Domain.DomainModels;

namespace TicketApp.Domain.DTO
{
    public class TicketAndList
    {
       public List<Ticket> TicketList { get; set; }
        public string Category { get; set; }
        public List<string> Categories { get; set; }
    }
}
