using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketApp.Domain.DomainModels;
using TicketApp.Domain.DTO;
using TicketApp.Repository.Interface;
using TicketApp.Services.Interface;
using static TicketApp.Repository.Interface.IRepository;

namespace TicketApp.Services.Implementation
{
   
        public class ShoppingCartService : IShoppingCartService
        {
            private readonly IRepository<ShoppingCart> _shoppingCartRepositorty;
            private readonly IRepository<Order> _orderRepositorty;
            private readonly IRepository<TicketInOrder> _ticketInOrderRepositorty;
            private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _mailRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<TicketInOrder> ticketInOrderRepositorty, IRepository<Order> orderRepositorty, IUserRepository userRepository,
            IRepository<EmailMessage> mailRepository)
            {
                _shoppingCartRepositorty = shoppingCartRepository;
                _userRepository = userRepository;
                _orderRepositorty = orderRepositorty;
                _ticketInOrderRepositorty = ticketInOrderRepositorty;
                _mailRepository = mailRepository;
            }

            public bool deleteTicketFromShoppingCart(string userId, Guid id)
            {
                if (!string.IsNullOrEmpty(userId) && id != null)
                {
                    //Select * from Users Where Id LIKE userId

                    var loggedInUser = this._userRepository.Get(userId);

                    var userShoppingCart = loggedInUser.UserCart;

                    var itemToDelete = userShoppingCart.TicketInShoppingCart.Where(z => z.TicketId.Equals(id)).FirstOrDefault();

                    userShoppingCart.TicketInShoppingCart.Remove(itemToDelete);

                    this._shoppingCartRepositorty.Update(userShoppingCart);

                    return true;
                }

                return false;
            }

            public ShoppingCartDto getShoppingCartInfo(string userId)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var Alltickets = userShoppingCart.TicketInShoppingCart.ToList();

                var allticketPrice = Alltickets.Select(z => new
                {
                    ticketPrice = z.Ticket.TicketPrice,
                    Quanitity = z.Quantity
                }).ToList();

                var totalPrice = 0;


                foreach (var item in allticketPrice)
                {
                    totalPrice += item.Quanitity * item.ticketPrice;
                }


                ShoppingCartDto scDto = new ShoppingCartDto
                {
                    Tickets = Alltickets,
                    TotalPrice = totalPrice
                };


                return scDto;

            }

            public bool orderNow(string userId)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    //Select * from Users Where Id LIKE userId

                    var loggedInUser = this._userRepository.Get(userId);

                    var userShoppingCart = loggedInUser.UserCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Successfully created order";
                mail.Status = false;

                Order order = new Order
                    {
                        Id = Guid.NewGuid(),
                        User = loggedInUser,
                        UserId = userId
                    };

                    this._orderRepositorty.Insert(order);

                    List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

                    var result = userShoppingCart.TicketInShoppingCart.Select(z => new TicketInOrder
                    {
                        Id = Guid.NewGuid(),
                        TicketId = z.Ticket.Id,
                        OrderedTicket = z.Ticket,
                        OrderId = order.Id,
                        UserOrder = order,
                        Quantity = z.Quantity
                    }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0;

                sb.AppendLine("Your order is completed. The order conains: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i-1];

                    totalPrice += item.Quantity * item.OrderedTicket.TicketPrice;

                    sb.AppendLine(i.ToString() + ". " + item.OrderedTicket.MovieName +" on DATE: "+ item.OrderedTicket.DateTime +" with price of: " + item.OrderedTicket.TicketPrice + " and quantity of: " + item.Quantity);
                }

                sb.AppendLine("Total price: " + totalPrice.ToString());


                mail.Content = sb.ToString();


                ticketInOrders.AddRange(result);

                    foreach (var item in ticketInOrders)
                    {
                        this._ticketInOrderRepositorty.Insert(item);
                    }

                    loggedInUser.UserCart.TicketInShoppingCart.Clear();

                    this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(mail);

                return true;
                }
                return false;
            }
        }
    }

