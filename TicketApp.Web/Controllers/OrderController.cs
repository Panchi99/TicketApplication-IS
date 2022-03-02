using GemBox.Document;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TicketApp.Domain.DomainModels;
using TicketApp.Domain.DTO;
using TicketApp.Domain.Identity;
using TicketApp.Services.Interface;

namespace TicketApp.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<TicketAppApplicationUser> userManager;

        public OrderController(IOrderService orderService, UserManager<TicketAppApplicationUser> userManager)
        {
            this._orderService = orderService;
            this.userManager = userManager;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult GetOrders()
        {
            List<Order> orders = this._orderService.getAllOrders();
            orders.RemoveAll(order => order.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(orders);
        }

       
        public IActionResult Details(Guid id)
        {
            return View(this._orderService.getOrderDetails(new BaseEntity {Id = id }));
        }

        public IActionResult CreateInvoice(Guid id)
        {

           var result = _orderService.getOrderDetails(new BaseEntity { Id = id});


            

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(),
                "template.docx");

            var document = DocumentModel.Load(templatePath);
            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{OrderName}}", result.User.UserName);

            StringBuilder sb = new StringBuilder();
            var totalPrice = 0.0;

            foreach (var item in result.TicketInOrder)
            {
                totalPrice += item.Quantity * item.OrderedTicket.TicketPrice;
                sb.AppendLine("Ticket for movie: " + item.OrderedTicket.MovieName + " with quantity of: " +
                    item.Quantity + " and price of: " + item.OrderedTicket.TicketPrice + ".\n Starting time:"
                    + item.OrderedTicket.DateTime);
            }
            document.Content.Replace("{{OrderedTickets}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString());

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");




        }

    }
}

