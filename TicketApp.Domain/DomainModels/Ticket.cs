using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TicketApp.Domain.DTO;

namespace TicketApp.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
        [Required]
        public string MovieName { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int TicketPrice { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public string Actors { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCart { get; set; }
        public IEnumerable<TicketInOrder> TicketInOrders { get; set; }

    }
}
