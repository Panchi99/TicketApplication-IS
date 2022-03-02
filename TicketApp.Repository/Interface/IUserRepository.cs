using System;
using System.Collections.Generic;
using System.Text;
using TicketApp.Domain.Identity;

namespace TicketApp.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<TicketAppApplicationUser> GetAll();
        TicketAppApplicationUser Get(string id);
        void Insert(TicketAppApplicationUser entity);
        void Update(TicketAppApplicationUser entity);
        void Delete(TicketAppApplicationUser entity);
    }
}
