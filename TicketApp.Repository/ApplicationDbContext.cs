using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TicketApp.Domain.DomainModels;
using TicketApp.Domain.DTO;
using TicketApp.Domain.Identity;

namespace TicketApp.Repository
{
    public class ApplicationDbContext : IdentityDbContext<TicketAppApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public virtual DbSet<TicketInShoppingCart> TIcketInShoppingCarts { get; set; }
        public virtual DbSet<TicketInOrder> TicketInOrder { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticket>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
             .Property(z => z.Id)
             .ValueGeneratedOnAdd();

            builder.Entity<TicketInShoppingCart>()
                .HasKey(z => new { z.TicketId, z.ShoppingCartId });

            builder.Entity<TicketInShoppingCart>()
               .HasOne(z => z.Ticket)
               .WithMany(z => z.TicketInShoppingCart)
               .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<TicketInShoppingCart>()
               .HasOne(z => z.ShoppingCart)
               .WithMany(z => z.TicketInShoppingCart)
               .HasForeignKey(z => z.TicketId);

            builder.Entity<ShoppingCart>()
                .HasOne(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);

            builder.Entity<TicketInOrder>()
               .HasKey(z => new { z.TicketId, z.OrderId });

            builder.Entity<TicketInOrder>()
                .HasOne(z => z.OrderedTicket)
                .WithMany(z => z.TicketInOrders)
                .HasForeignKey(z => z.OrderId);

            builder.Entity<TicketInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.TicketInOrder)
                .HasForeignKey(z => z.TicketId);


        }

    }
}
