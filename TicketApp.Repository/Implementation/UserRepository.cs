using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketApp.Domain.Identity;
using TicketApp.Repository.Interface;

namespace TicketApp.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<TicketAppApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<TicketAppApplicationUser>();
        }
        public IEnumerable<TicketAppApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public TicketAppApplicationUser Get(string id)
        {
            return entities
               .Include(z => z.UserCart)
               .Include("UserCart.TicketInShoppingCart")
               .Include("UserCart.TicketInShoppingCart.Ticket")
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(TicketAppApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(TicketAppApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(TicketAppApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
